using UnityEngine;

namespace POLYGONWARE.Common.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraSortMode : MonoBehaviour
    {
        [SerializeField] private TransparencySortMode _sortMode = TransparencySortMode.CustomAxis;
        [SerializeField] private Vector3 _sortAxis = Vector2.up;
        private void Awake()
        {
            var currentCamera = GetComponent<UnityEngine.Camera>();
            currentCamera.transparencySortMode = _sortMode;
            currentCamera.transparencySortAxis = _sortAxis;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            var currentCamera = GetComponent<UnityEngine.Camera>();
            currentCamera.transparencySortMode = _sortMode;
            currentCamera.transparencySortAxis = _sortAxis;
        }
        #endif
    }
}