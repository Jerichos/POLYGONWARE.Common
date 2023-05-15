using System;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class CellLineRenderer : MonoBehaviour
    {
        [SerializeField] private Gradient _color;
        [SerializeField] private float _width;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Vector3[] _positions;
        [SerializeField] private Material _material;

        public void SetLine(Vector3[] positions, Gradient color)
        {
            _positions = positions;
            
            if (positions.Length < 4)
            {
                Debug.LogError("CellLineRenderer must have at least 4 points");
                return;
            }

            _lineRenderer.colorGradient = color;
            _lineRenderer.startWidth = _width;
            _lineRenderer.endWidth = _width;
            _lineRenderer.SetPositions(positions);
        }
        
        private void OnValidate()
        {
            _lineRenderer.material = _material;
            SetLine(_positions, _color);
        }
    }
}