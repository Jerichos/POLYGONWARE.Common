using POLYGONWARE.Common.Player;
using POLYGONWARE.Common.Util;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public abstract class Controller : MonoBehaviour, IController
    {
        [SerializeField] protected Controllable _defaultControllable;

        private IControllable _controllable;
        public IControllable Controllable => _controllable;

        public event GenericDelegate<IControllable> EControllableChanged;

        protected virtual void Awake()
        {
            if (_defaultControllable)
            {
                _controllable = _defaultControllable;
                HandleControl(_controllable);
            }
        }

        public void Control(IControllable character)
        {
            _controllable = character;
            HandleControl(character);
            Debug.Log("EControllableChanged");
            EControllableChanged?.Invoke(_controllable);
        }

        protected abstract void HandleControl(IControllable controllable);
    }
}