using POLYGONWARE.Common.Game;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Controller _controller;

        private Controllable _controllable;

        public static Player LocalPlayer;
        
        private void Start()
        {
            LocalPlayer = this;
            //LevelManager.Instance.AddPlayer(this);
        }

        public void AssignToControl(Controllable character)
        {
            _controller.Control(character);
        }
    }
}