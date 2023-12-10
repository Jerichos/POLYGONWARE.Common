using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.Combat
{
public class HitProxy : MonoBehaviour, IHittable
{
    public GenericDelegate<DamageData> OnHit;
    
    public void Hit(DamageData damageData)
    {
        OnHit?.Invoke(damageData);
    }
}
}