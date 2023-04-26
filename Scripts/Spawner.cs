using System;
using POLYGONWARE.Common.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace POLYGONWARE.Common
{
public class Spawner : MonoBehaviour
{
    private Object _prefab;
    private CooldownTimer _timer;

    private Vector3 _spawnPosition;
    private int _spawnCount;
    private int _spawned;

    private GenericDelegate<Object> _callback;

    private void Update()
    {
        if (_timer.TimePassed())
        {
            Spawn();
            _timer.Restart();
            _spawned++;

            if (_spawned >= _spawnCount)
                enabled = false;
        }
    }

    private void Spawn()
    {
        var instance = Instantiate(_prefab, _spawnPosition, Quaternion.identity);
        instance.name = instance.name + "_" + _spawned;
        _callback?.Invoke(instance);
    }

    public void StartSpawning(Object prefab, int spawnCount, Vector3 position, float spawnCooldown, GenericDelegate<Object> callback)
    {
        enabled = true;
        _spawnPosition = position;
        _prefab = prefab;
        _spawnCount = spawnCount;
        _spawned = 0;
        _callback = callback;

        _timer = new CooldownTimer(spawnCooldown);
    }
}
}