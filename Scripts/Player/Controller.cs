using UnityEngine;

namespace Common
{
    public abstract class Controller : MonoBehaviour, IController
    {
        [SerializeField] protected Controllable _controllable;

        public void Control(Controllable character)
        {
            _controllable = character;
            HandleControl(character);
        }

        protected abstract void HandleControl(Controllable controllable);
        
    }
}