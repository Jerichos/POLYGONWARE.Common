using UnityEngine;
using UnityEngine.InputSystem;

namespace Common
{
    public class Controllable : MonoBehaviour, IControllable
    {
        [SerializeField][HideInInspector] private InputActionAsset _actionAsset;
        [SerializeField][HideInInspector] private string _actionMap;

        public Transform Transform => transform;
        public IController Controller { get; private set; }
        public string GetActionMap => _actionMap;

        public virtual void TakeControl(IController owner)
        {
            Controller = owner;
        }

#if UNITY_EDITOR
        [SerializeField] public string ActionMap
        {
            get => _actionMap;
            set => _actionMap = value;
        }

        [SerializeField] public InputActionAsset ActionAsset
        {
            get => _actionAsset;
            set => _actionAsset = value;
        }
#endif
    }
}