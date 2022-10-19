using POLYGONWARE.Common.Camera;
using UnityEngine;
using UnityEngine.Events;

namespace POLYGONWARE.Common.Game
{
    public class OnScreenTrigger : MonoBehaviour, IOnScreenTrigger
    {
        [SerializeField] private bool _triggerOnce = true;
        [SerializeField] private UnityEvent _onScreenEvent;
        [SerializeField] private UnityEvent _onScreenExit;

        private bool _triggered;
        
        public void OnScreenEnter()
        {
            Debug.Log("1 ENTER");
            if(_triggerOnce && _triggered)
                return;
            
            _triggered = true;
            _onScreenEvent?.Invoke();
        }

        public void OnScreenExit()
        {
            Debug.Log("2 EXIT");
            _onScreenExit?.Invoke();
        }
    }
}