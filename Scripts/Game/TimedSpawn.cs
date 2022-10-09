using System;
using Unity.Mathematics;
using UnityEngine;

namespace Common
{
    public class TimedSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnPfb;
        [SerializeField] private float _cooldown = 1;
        [SerializeField] private float _startDelay = 0;

        private void OnEnable()
        {
            InvokeRepeating(nameof(SpawnPrefab), _startDelay, _cooldown);
        }

        private void SpawnPrefab()
        {
            Instantiate(_spawnPfb, transform.position, Quaternion.identity);
        }
    }
}