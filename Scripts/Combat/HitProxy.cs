using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common.Combat
{

public delegate bool HitDelegate(DamageData damageData);

public class HitProxy : MonoBehaviour, IHittable
{
    public HitDelegate OnHit;
    
    public bool Hit(DamageData damageData)
    {
        if(OnHit == null) 
            return false;
        
        return OnHit.Invoke(damageData);
    }
}
}