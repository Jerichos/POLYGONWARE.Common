using System;
using UnityEngine;

namespace Common
{
    public abstract class Controller : MonoBehaviour, IController
    {
        [SerializeField] protected Controllable _controllable;

        protected virtual void Awake()
        {
            if(_controllable)
                HandleControl(_controllable);
        }

        public void Control(Controllable character)
        {
            _controllable = character;
            HandleControl(character);
        }

        protected abstract void HandleControl(Controllable controllable);
        
    }
}