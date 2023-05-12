using UnityEngine;

namespace POLYGONWARE.Common
{
    [RequireComponent(typeof(BoxPhysics2D))]
    public class Gravity2D : MonoBehaviour
    {
        [SerializeField] private Vector2 _gravityForce = Vector2.down * 5;

        [SerializeField] [HideInInspector] private BoxPhysics2D _boxPhysics;

        private void Update()
        {
            _boxPhysics.AddVelocity(_gravityForce);
        }

        private void Reset()
        {
            _boxPhysics = GetComponent<BoxPhysics2D>();
        }
    }
}