using UnityEngine;

namespace POLYGONWARE.Common.Player
{
    public abstract class Controller : MonoBehaviour, IController
    {
        [SerializeField] protected Controllable _defaultControllable;

        protected IControllable _controllable;

        protected virtual void Awake()
        {
            if (_defaultControllable)
            {
                _controllable = _defaultControllable;
                TakeControl(_controllable);
            }
        }

        public void Control(IControllable character)
        {
            _controllable = character;
            TakeControl(character);
        }

        public abstract void TakeControl(IControllable controllable);
    }
}