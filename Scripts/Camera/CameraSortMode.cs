using System;
using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(Camera))]
    public class CameraSortMode : MonoBehaviour
    {
        [SerializeField] private TransparencySortMode _sortMode = TransparencySortMode.CustomAxis;
        [SerializeField] private Vector3 _sortAxis = Vector2.up;
        private void Awake()
        {
            var currentCamera = GetComponent<Camera>();
            currentCamera.transparencySortMode = _sortMode;
            currentCamera.transparencySortAxis = _sortAxis;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            var currentCamera = GetComponent<Camera>();
            currentCamera.transparencySortMode = _sortMode;
            currentCamera.transparencySortAxis = _sortAxis;
        }
        #endif
    }
}