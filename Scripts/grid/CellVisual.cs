using UnityEngine;

namespace POLYGONWARE.Common
{
    public class CellVisual : GridEntity
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Color _validColor = Color.green;
        [SerializeField] private Color _invalidColor = Color.red;

        public void SetValid(bool isValid)
        {
            if (isValid)
            {
                _lineRenderer.startColor = _validColor;
                _lineRenderer.endColor = _validColor;
            }
            else
            {
                _lineRenderer.startColor = _invalidColor;
                _lineRenderer.endColor = _invalidColor;
            }
        }

        public virtual void Show()
        {
            _lineRenderer.enabled = true;
        }

        public virtual void Hide()
        {
            _lineRenderer.enabled = false;
        }
    }
}