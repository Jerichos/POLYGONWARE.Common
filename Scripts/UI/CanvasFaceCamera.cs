using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class CanvasFaceCamera : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            AlignWithCamera();
            //transform.rotation = Quaternion.LookRotation((_camera.transform.position - transform.position).normalized);
        }

        private void AlignWithCamera()
        {
            transform.rotation = _camera.transform.rotation;
        }

        private void OnValidate()
        {
            if(!_camera)
                _camera= UnityEngine.Camera.main;
            
            AlignWithCamera();
        }
    }
}