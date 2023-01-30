using System;
using POLYGONWARE.Common.Combat;
using POLYGONWARE.Common.Pooling;
using UnityEngine;

namespace POLYGONWARE.Common
{
public class TargetProjectile : PooledObject
{
    [SerializeField] private float _speed = 2;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private GameObject _model;
    [SerializeField] private TrailRenderer _trail; // TODO: move to subclass specific for trail

    private DamageData _damageData;
    private Vector3 _targetPosition;

    private void Update()
    {
        if (_targetTransform)
            _targetPosition = _targetTransform.position;
        
        var position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        transform.position = position;

        if (transform.position == _targetPosition)
        {
            OnProjectileHit();
        }
    }

    private void OnProjectileHit()
    {
        if (_targetTransform && _damageData.Target is IHittable hittable)
        {
            hittable.Hit(_damageData);
        }
        
        enabled = false;
        
        if(_model)
            _model.SetActive(false);
        
        if(_trail)
            Invoke(nameof(DeactivateMe), _trail.time);
        else
            DeactivateMe();

        // Debug.Log("OnProjectileHit");
    }

    private void DeactivateMe()
    {
        gameObject.SetActive(false);
        PoolSystem.ReturnInstance(this);
    }

    public void Fire(DamageData damageData)
    {
        _targetTransform = damageData.Target.transform;
        _damageData = damageData;
        gameObject.SetActive(true);
    }
    
    public void Fire(DamageData damageData, float speed)
    {
        _speed = speed;
        Fire(damageData);
    }
}
}