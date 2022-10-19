using POLYGONWARE.Common.Camera;
using UnityEngine;

namespace POLYGONWARE.Common.Player
{
    public abstract class PlayerController : Controller
    {
        [SerializeField] protected PlayerCamera _playerCamera;
        
        public static PlayerController LocalPlayerController;
        
        protected override void Awake()
        {
            if (LocalPlayerController)
            {
                //TODO: What if more players play on one machine?
                Debug.LogWarning("There can be only one local player controller.");
                Destroy(gameObject);
                return;
            }
            
            base.Awake();
            LocalPlayerController = this;
        }

        
    }
}