﻿using System;
using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace POLYGONWARE.Common
{
    public class FollowPath : MonoBehaviour
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private PathManager _pathManager;
        [FormerlySerializedAs("_pathPoint")] [SerializeField] private int _currentPoint = 0;
        [SerializeField] private bool _loop = false;
        [SerializeField] private bool _forward = true;

        private IFollowPath _followPath;

        private GenericDelegate<GameObject> _pathEndedCallback;
        
        public void NotifyPathEnded(GenericDelegate<GameObject> callback)
        {
            _pathEndedCallback = callback;
        }

        public PathManager Path
        {
            set => _pathManager = value;
        }
        
        private void Start()
        {
            transform.position = _pathManager.Path.Nodes[_currentPoint].Vector3;

            if (_forward)
                _followPath = new ForwardFollow(_currentPoint, _pathManager.Path.Nodes.Count);
            else
                _followPath = new BackwardFollow(_currentPoint, _pathManager.Path.Nodes.Count);
        }

        private void Update()
        {
            var endPosition = _pathManager.GetPosition(_followPath.GetNextPoint());

            var position = Vector3.MoveTowards(transform.position, endPosition, _speed * Time.deltaTime);

            transform.position = position;

            if (endPosition == position)
            {
                _followPath.SetNextPoint();
                
                if (_followPath.IsEndOfPath())
                {
                    _pathEndedCallback?.Invoke(gameObject);
                    
                    if (_loop)
                        _followPath = _followPath.GetNextFollowPath();
                    else
                        enabled = false;
                }
            }
        }
    }

    public interface IFollowPath
    {
        void SetNextPoint();
        bool IsEndOfPath();
        int GetNextPoint();

        IFollowPath GetNextFollowPath();
    }

    public struct ForwardFollow : IFollowPath
    {
        private int CurrentPoint;
        private int NextPoint;
        private readonly int PointCount;
        
        public ForwardFollow(int currentPoint, int pointCount)
        {
            PointCount = pointCount;
            CurrentPoint = currentPoint;
            NextPoint = currentPoint + 1;
        }

        public void SetNextPoint()
        {
            CurrentPoint = NextPoint;
            NextPoint++;
        }

        public bool IsEndOfPath()
        {
            if (NextPoint >= PointCount)
            {
                return true;
            }

            return false;
        }

        public int GetNextPoint()
        {
            return NextPoint;
        }

        public IFollowPath GetNextFollowPath()
        {
            return new BackwardFollow(CurrentPoint, PointCount);
        }
    }

    public struct BackwardFollow : IFollowPath
    {
        private int CurrentPoint;
        private int NextPoint;
        private readonly int PointCount;

        public BackwardFollow(int currentPoint, int pointCount)
        {
            CurrentPoint = currentPoint;
            PointCount = pointCount;
            NextPoint = currentPoint - 1;
        }
        
        public bool IsEndOfPath()
        {
            if (NextPoint <= -1)
            {
                return true;
            }
            
            return false;
        }

        public void SetNextPoint()
        {
            CurrentPoint = NextPoint;
            NextPoint--;
        }

        public int GetNextPoint()
        {
            return NextPoint;
        }

        public IFollowPath GetNextFollowPath()
        {
            return new ForwardFollow(CurrentPoint, PointCount);
        }
    }
}