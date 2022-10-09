using UnityEngine;

namespace Common
{
    public class Controllable : MonoBehaviour, IControllable
    {
        public IController Owner { get; private set; }
        public virtual void TakeControl(IController owner)
        {
            Owner = owner;
        }
    }
}