﻿using System;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasWorldScaler : MonoBehaviour
    {
        [SerializeField] private int _pixelsPerUnit = 256;

        [SerializeField] private float _unitySizeX = 1;
        [SerializeField] private float _unitySizeY = 0.5f;

        [SerializeField] private bool _invertX = false;

        public bool InvertX
        {
            set
            {
                _invertX = value;
                var scale = transform.localScale;
                if (_invertX)
                {
                    scale.x = Mathf.Abs(scale.x) * -1;
                }
                else
                {
                    scale.x = Mathf.Abs(scale.x);
                }

                transform.localScale = scale;
            }
        }
        
        private void OnValidate()
        {
            Canvas _canvas = GetComponent<Canvas>();
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.worldCamera = UnityEngine.Camera.main;

            var scale = _canvas.transform.localScale;
            scale.x = 1f / _pixelsPerUnit;
            scale.y = 1f / _pixelsPerUnit;

            float width = _pixelsPerUnit * _unitySizeX;
            float height = _pixelsPerUnit * _unitySizeY;

            var rect = _canvas.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(width, height);

            _canvas.transform.localScale = scale;

            InvertX = _invertX;
        }
    }
}