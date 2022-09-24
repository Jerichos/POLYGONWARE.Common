using UnityEngine;

namespace Common
{
    public class Controllable : MonoBehaviour, IControllable
    {
        public virtual void TakeControl(IControllable owner)
        {
            
        }
    }
}