using UnityEngine;

namespace POLYGONWARE.Common.Util
{
public class FixRotation : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(_rotation);
    }
}
}