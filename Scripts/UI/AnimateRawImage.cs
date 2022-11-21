using System;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Common.UI
{
    public class AnimateRawImage : MonoBehaviour
    {
        [SerializeField] private float _verticalSpeed = 1;
        [SerializeField] private float _horizontalSpeed = 1;
        [SerializeField] private RawImage _rawImage;

        private void Update()
        {
            var rect = _rawImage.uvRect;
            rect.x += _horizontalSpeed * Time.deltaTime;
            rect.y += _verticalSpeed * Time.deltaTime;
            _rawImage.uvRect = rect;
        }
    }
}