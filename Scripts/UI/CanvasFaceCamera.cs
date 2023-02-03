using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class CanvasFaceCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        private void Update()
        {
            AlignWithCamera();
        }

        private void AlignWithCamera()
        {
            transform.rotation = _cameraTransform.transform.rotation;
        }

        // private void OnValidate()
        // {
        //     
        //     if(!_cameraTransform)
        //         _cameraTransform= GameObject.FindWithTag("MainCamera").transform;
        //     
        //     AlignWithCamera();
        // }
    }
}