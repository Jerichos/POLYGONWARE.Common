using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Common
{
    public class GlobalSortMode : Singleton<GlobalSortMode>
    {
        [SerializeField] private TransparencySortMode _sortMode = TransparencySortMode.CustomAxis;
        [SerializeField] private Vector3 _sortAxis = Vector2.up;

        protected override void Awake()
        {
            base.Awake();
            GraphicsSettings.transparencySortMode = _sortMode;
            GraphicsSettings.transparencySortAxis = _sortAxis;
        }

        private void OnValidate()
        {
            Debug.Log("Sort changed " + _sortAxis + " mode: " + _sortMode);
            GraphicsSettings.transparencySortMode = _sortMode;
            GraphicsSettings.transparencySortAxis = _sortAxis;
        }
    }
}