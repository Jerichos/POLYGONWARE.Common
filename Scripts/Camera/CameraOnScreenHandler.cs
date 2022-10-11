using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class CameraOnScreenHandler : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _hitLayer;
        [SerializeField] float _angle = 0;
        [SerializeField] private Vector2 _direction = Vector2.right;

        [SerializeField] private Vector2 _size;
        private int _count;

        private float _distance;
        private Vector3 _origin;

        private readonly RaycastHit2D[] _results = new RaycastHit2D[25]; //!! Length is the limit of triggers can appear on the screen at one time !!
        private readonly List<TriggerData> _onScreenTriggers = new();

        private void Awake()
        {
            var aspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;
            var orthoSize = _camera.orthographicSize;
            
            _size.x = 2.0f * orthoSize * aspect;
            _size.y = 2.0f * _camera.orthographicSize;

            _distance = _size.x;
        }

        // TODO: What about making rigidbody and check OnTriggerEnter/Exit instead?
        private void TriggerOnScreen()
        {
            Debug.Log(name + " BoxCastNonAlloc");
            _origin = _camera.transform.position;
            _origin.x -= _size.x;
            _count = Physics2D.BoxCastNonAlloc(_origin, _size, _angle, _direction, _results, _distance, _hitLayer);

            for (int i = 0; i < _count; i++)
            {
                var instanceID = _results[i].transform.GetInstanceID();
                bool add = true;

                for (int j = 0; j < _onScreenTriggers.Count; j++)
                {
                    if (_onScreenTriggers[j].ID == instanceID)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                {
                    var trigger = _results[i].transform.GetComponent<IOnScreenTrigger>();
                    trigger.OnScreenEnter();
                    TriggerData data = new TriggerData { ID = instanceID, Trigger = trigger};
                    _onScreenTriggers.Add(data);
                }
            }

            for (int i = 0; i < _onScreenTriggers.Count; i++)
            {
                bool remove = true;
                for (int j = 0; j < _count; j++)
                {
                    if (_onScreenTriggers[i].ID == _results[j].transform.GetInstanceID())
                    {
                        remove = false;
                        break;
                    }
                }

                if (remove)
                {
                    _onScreenTriggers[i].Trigger.OnScreenExit();
                    _onScreenTriggers.RemoveAt(i);
                }
            }
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(TriggerOnScreen), 0, 1);
        }

        private void OnDisable()
        {
            _onScreenTriggers.Clear();
            
            
            CancelInvoke();
        }

        private void OnDrawGizmos()
        {
            var origin = (Vector2)_camera.transform.position;
            origin.x -= _size.x;
            var aspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;
            var orthoSize = _camera.orthographicSize;
            
            _size.x = 2.0f * orthoSize * aspect;
            _size.y = 2.0f * _camera.orthographicSize;
            _distance = _size.x;

            Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
            float w = _size.x * 0.5f;
            float h = _size.y * 0.5f;
            p1 = new Vector2(-w, h);
            p2 = new Vector2(w, h);
            p3 = new Vector2(w, -h);
            p4 = new Vector2(-w, -h);

            Quaternion q = Quaternion.AngleAxis(_angle, new Vector3(0, 0, 1));
            p1 = q * p1;
            p2 = q * p2;
            p3 = q * p3;
            p4 = q * p4;

            p1 += origin;
            p2 += origin;
            p3 += origin;
            p4 += origin;

            Vector2 realDistance = _direction.normalized * _distance;
            p5 = p1 + realDistance;
            p6 = p2 + realDistance;
            p7 = p3 + realDistance;
            p8 = p4 + realDistance;


            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);

            Gizmos.DrawLine(p5, p6);
            Gizmos.DrawLine(p6, p7);
            Gizmos.DrawLine(p7, p8);
            Gizmos.DrawLine(p8, p5);

            Gizmos.DrawLine(p1, p5);
            Gizmos.DrawLine(p2, p6);
            Gizmos.DrawLine(p3, p7);
            Gizmos.DrawLine(p4, p8);
        }
    }

    public struct TriggerData
    {
        public int ID;
        public IOnScreenTrigger Trigger;
    }
}