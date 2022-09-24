using System.Collections;
using System.Collections.Generic;
using Contra;
using UnityEngine;

namespace Common
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private BoxCollider2D _levelBounds;
        [SerializeField] private Transform _respawn;
        [SerializeField] private Controllable _characterPrefab;
        
        public BoxCollider2D LevelBounds => _levelBounds;

        public readonly List<Player> Players = new();

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(Freeze(3));
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
            
            // Spawn Character For a Player
            var character = Instantiate(_characterPrefab);
            _characterPrefab.transform.position = _respawn.position;
            
            // Assign character to a Player
            player.AssignToControl(character);
        }

        private IEnumerator Freeze(int frames)
        {
            Time.timeScale = 0;

            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            
            Time.timeScale = 1;
        }
    }
}