using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public class PlayerController : Controller
    {
        [SerializeField] protected PlayerCamera _playerCamera;

        private PlayerControls _playerControls;
        
        public static PlayerController Local;
        
        private IInputHandler _inputHandler;
        
        protected override void Awake()
        {
            _playerControls = new PlayerControls();
            
            if (Local)
            {
                //TODO: What if more players play on one machine?
                Debug.LogWarning("There can be only one local player controller.");
                Destroy(gameObject);
                return;
            }
            
            base.Awake();
            Local = this;
        }

        protected override void HandleControl(IControllable controllable)
        {
            controllable.TakeControl(this);
            _inputHandler = (IInputHandler)Activator.CreateInstance(controllable.InputHandlerType, _playerControls, controllable);
        }
    }
}