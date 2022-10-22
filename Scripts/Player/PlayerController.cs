using POLYGONWARE.Common.Camera;
using POLYGONWARE.Common.Player;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public abstract class PlayerController : Controller
    {
        [SerializeField] protected PlayerCamera _playerCamera;
        
        public static PlayerController Local;
        
        protected override void Awake()
        {
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

        
    }
}