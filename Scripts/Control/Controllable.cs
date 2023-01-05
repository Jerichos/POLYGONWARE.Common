using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{
    public abstract class Controllable : MonoBehaviour, IControllable
    {
        [SerializeField][HideInInspector] private InputActionAsset _actionAsset;
        [SerializeField][HideInInspector] private string _actionMap;

        public Transform Transform => transform;
        public IController Controller { get; private set; }
        public string GetActionMap => _actionMap;

        public virtual void TakeControl(IController owner)
        {
            Debug.Log(name + " TakeControl ");
            Controller = owner;
        }

        public virtual Type InputHandlerType => typeof(Controllable);
        
        //public abstract Type InputHandlerType { get; }

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