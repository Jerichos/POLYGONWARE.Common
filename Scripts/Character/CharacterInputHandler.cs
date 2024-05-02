using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public class CharacterInputHandler : IInputHandler
    {
        private readonly CharacterManager _character;
        private readonly PlayerControls.CharacterActions _characterActions;

        public CharacterInputHandler(PlayerControls playerControls, IControllable controllable)
        {
            if (controllable is not CharacterManager character) 
                return;
            
            _character = character;
            _characterActions = playerControls.Character;

            _characterActions.Movement.performed += OnMovementPerformed;
            _characterActions.Movement.started += OnMovementStarted;
            _characterActions.Movement.canceled += OnMovementCanceled;

            _characterActions.Jump.started += OnJumpStarted;
            _characterActions.Jump.canceled += OnJumpStopped;
            
            playerControls.Enable();
            _characterActions.Enable();
            
            Debug.Log("Handler created.");
        }

        private void OnJumpStopped(InputAction.CallbackContext obj)
        {
            _character.Physics.JumpStop();
        }

        private void OnJumpStarted(InputAction.CallbackContext obj)
        {
            _character.Physics.JumpStart();
        }

        private void OnMovementStarted(InputAction.CallbackContext obj)
        {
            // Debug.Log("Movement Started " + obj.ReadValue<Vector2>());
        }

        private void OnMovementPerformed(InputAction.CallbackContext obj)
        {
            // Debug.Log("Movement Performed " + obj.ReadValue<Vector2>());
            _character.Physics.Move(obj.ReadValue<Vector2>());
        }
        
        private void OnMovementCanceled(InputAction.CallbackContext obj)
        {
            // Debug.Log("Movement Canceled " + obj.ReadValue<Vector2>());
            _character.Physics.StopMove();
        }

        public void Dispose()
        {
            _characterActions.Movement.performed -= OnMovementPerformed;
            _characterActions.Movement.started -= OnMovementStarted;
            _characterActions.Movement.canceled -= OnMovementCanceled;
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }
    }
}