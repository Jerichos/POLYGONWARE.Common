using UnityEngine;

namespace Common
{
    public static class AoeDamage2D
    {
        private static RaycastHit2D[] _results = new RaycastHit2D[10];
        
        public static void CreateAOE(DamageData damageData, Vector2 position, float radius, LayerMask _hitLayer)
        {
            var hits = Physics2D.CircleCastNonAlloc(position, radius, Vector2.up, _results, radius, _hitLayer);
            for (int i = 0; i < hits; i++)
            {
                var hittable = _results[i].transform.GetComponent<IHittable>();
                hittable?.Hit(damageData);
            }
        }
    }
}