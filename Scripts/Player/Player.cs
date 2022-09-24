using System;
using Contra;
using UnityEngine;

namespace Common
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Controller _controller;

        private Controllable _controllable;
        
        private void Start()
        {
            LevelManager.Instance.AddPlayer(this);
        }

        public void AssignToControl(Controllable character)
        {
            _controller.Control(character);
        }
    }
}