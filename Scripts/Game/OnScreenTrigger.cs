using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class OnScreenTrigger : MonoBehaviour, IOnScreenTrigger
    {
        [SerializeField] private bool _triggerOnce = true;
        [SerializeField] private UnityEvent _onScreenEvent;
        [SerializeField] private UnityEvent _onScreenExit;

        private bool _triggered;
        
        public void OnScreenEnter()
        {
            if(_triggerOnce && _triggered)
                return;
            
            _triggered = true;
            _onScreenEvent?.Invoke();
        }

        public void OnScreenExit()
        {
            _onScreenExit?.Invoke();
        }
    }
}