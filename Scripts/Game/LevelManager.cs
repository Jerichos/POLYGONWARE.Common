using System.Collections;
using System.Collections.Generic;
using POLYGONWARE.Common.Player;
using UnityEngine;

namespace POLYGONWARE.Common.Game
{
    [DefaultExecutionOrder(-10)]
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private BoxCollider2D _levelBounds;
        [SerializeField] private SpawnPoint2D _spawnPoint;
        [SerializeField] private Controllable _characterPrefab;
        
        public BoxCollider2D LevelBounds => _levelBounds;

        public readonly List<Player.Player> Players = new();

        private SpawnPoint2D[] _spawnPoints;
        
        protected override void Awake()
        {
            base.Awake();

            _spawnPoints = GetComponentsInChildren<SpawnPoint2D>();
            
            StartCoroutine(Freeze(4));
        }

        public void AddPlayer(Player.Player player)
        {
            Players.Add(player);
            
            // Spawn Character For a Player
            var character = Instantiate(_characterPrefab, _spawnPoint.Position, Quaternion.identity);
            Debug.Log("Spawn Point: " + _spawnPoint.Position);
            
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

        public void Respawn(Player.Player player)
        {
            // Spawn Character For a Player
            var character = Instantiate(_characterPrefab, _spawnPoint.Position, Quaternion.identity);
            Debug.Log("Spawn Point: " + _spawnPoint.Position);
            
            // Assign character to a Player
            player.AssignToControl(character);
        }

        private void OnEnable()
        {
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                _spawnPoints[i].ETriggered += OnSpawnPointTriggered;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                _spawnPoints[i].ETriggered -= OnSpawnPointTriggered;
            }
        }

        private void OnSpawnPointTriggered(SpawnPoint2D spawnPoint)
        {
            if (_spawnPoint)
                _spawnPoint.Active = false;
            
            _spawnPoint = spawnPoint;
            _spawnPoint.Active = true;
        }

        public void SetSpawnPoint(SpawnPoint2D spawnPoint)
        {
            if (_spawnPoint)
                _spawnPoint.Active = false;
            
            _spawnPoint = spawnPoint;
            _spawnPoint.Active = true;
        }
    }
}