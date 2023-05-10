using POLYGONWARE.Common.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace POLYGONWARE.Common
{
    public class FollowPath : MonoBehaviour
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private Path _path;
        [SerializeField] private int _currentPoint = 0;
        [SerializeField] private bool _loop = false;
        [SerializeField] private bool _forward = true;

        private IFollowPath _followPath;

        //public event GenericDelegate<GameObject> EPathEnded;
        public VoidDelegate PathEndCallback;

        public void SetPath(Path path)
        {
            _path = path;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public void Stop()
        {
            enabled = false;
        }

        public void StartFollow(Path path)
        {
            _path = path;
            enabled = true;
        }
        
        private void Start()
        {
            transform.position = _path.GetPosition(0);

            if (_forward)
                _followPath = new ForwardFollow(_currentPoint, _path.Nodes.Length);
            else
                _followPath = new BackwardFollow(_currentPoint, _path.Nodes.Length);
        }

        private void Update()
        {
            var endPosition = _path.GetNextPoint(_currentPoint);

            var position = Vector3.MoveTowards(transform.position, endPosition, _speed * Time.deltaTime);

            transform.position = position;

            if (endPosition == position)
            {
                _currentPoint = _followPath.SetNextPoint();
                
                if (_followPath.IsEndOfPath())
                {
                    OnPathEnded();
                    //EPathEnded?.Invoke(gameObject);
                    PathEndCallback?.Invoke();
                    
                    if (_loop)
                        _followPath = _followPath.GetNextFollowPath();
                    else
                        enabled = false;
                }
            }
        }

        protected virtual void OnPathEnded()
        {
            
        }
    }

    public interface IFollowPath
    {
        int SetNextPoint();
        bool IsEndOfPath();

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

        public int SetNextPoint()
        {
            CurrentPoint = NextPoint;
            NextPoint++;
            return CurrentPoint;
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

        public int SetNextPoint()
        {
            CurrentPoint = NextPoint;
            NextPoint--;
            return CurrentPoint;
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