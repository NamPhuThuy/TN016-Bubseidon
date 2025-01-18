using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private EnemyController _target;
    [SerializeField] private float _speed;
    public float damage;
    
    public EnemyController Target
    {
        get => _target;
        set => _target = value;
    }
    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            Vector3 direction = _target.transform.position - transform.position;
            float distanceThisFrame = Time.deltaTime * 5;

            if (direction.magnitude <= distanceThisFrame)
            {
                _target.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
            transform.Translate(direction.normalized * distanceThisFrame*_speed, Space.World);
        } 
        else
        {
            Destroy(gameObject);
        }
        HandleDirection();
    }
    private void HandleDirection()
    {
        Vector3 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }
}
