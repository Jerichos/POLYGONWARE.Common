using UnityEngine;

namespace Common
{
    public static class PhysicsExtensions
    {
        public static void Clear(this BoxPhysics2D[] hits)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                hits[i] = null;
            }
        }

        public static BoxPhysics2D Add(this BoxPhysics2D[] hits, int instanceID)
        {
            // int freeID = -1;
            
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i])
                {
                    if(hits[i].ID == instanceID)
                        return null;
                    
                    continue;
                }

                
                var boxPhysics = PhysicsWorld2D.Get(instanceID);
                hits[i] = boxPhysics;
                Debug.Log("added " + boxPhysics.name);
                return boxPhysics;
            }

            // if (freeID != -1)
            // {
            //     var boxPhysics = PhysicsWorld2D.Get(instanceID);
            //     hits[freeID] = boxPhysics;
            //     return boxPhysics;
            // }
            
            // Debug.LogWarning("No space in hits.");
            return null;
        }
    }
}