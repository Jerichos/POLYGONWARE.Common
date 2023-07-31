using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public class PlayerController : Controller
    {
        [SerializeField] private InputActionAsset _inputAsset;
        [SerializeField] protected PlayerCamera _playerCamera;

        public static PlayerController Local;

        //private PlayerControls _playerInput;
        private IInputHandler _inputHandler;
        
        protected override void Awake()
        {
            if (Local)
            {
                //TODO: What if more players play on one machine?
                Debug.LogWarning("There can be only one local player controller.");
                Destroy(gameObject);
                return;
            }
            
            //_playerInput = new PlayerControls();
            base.Awake();
            Local = this;
        }

        protected override void HandleControl(IControllable controllable)
        {
            controllable.TakeControl(this);
            Debug.Log("type " + controllable.InputHandlerType);
            _inputHandler?.Dispose();
            _inputHandler = (IInputHandler)Activator.CreateInstance(controllable.InputHandlerType, _inputAsset, controllable);
            
            if(_playerCamera)
                _playerCamera.SetTarget(controllable.Transform);
        }
    }
}