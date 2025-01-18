using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeController : MonoBehaviour
{
    [SerializeField] private TowerController _tower;
    [SerializeField] private float _attackRange = 1.5f;
    
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private EnemyController _currentTarget;

    private Transform _transform;
    void Start()
    {
        _tower = GetComponentInParent<TowerController>();
        _circleCollider2D.radius = _attackRange;
        _transform = transform;
    }
    
    private void Update()
    {
        if (_currentTarget != null)
            return;
        
        _currentTarget = FindClosestEnemy();
    }
    
    private EnemyController FindClosestEnemy()
    {
        var minDistance = float.MaxValue;
        EnemyController target = null;
        
        foreach (EnemyController enemy in GamePlayManager.Instance.EnemyList)
        {
            float sqrMagnitude = Vector2.SqrMagnitude(_transform.position - enemy.transform.position);
            if (sqrMagnitude < minDistance)
            {
                minDistance = sqrMagnitude;
                target = enemy.gameObject.GetComponent<EnemyController>();
            }
        }

        return target;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"TNam - {other.transform.name} in tower range");
        switch (other.tag)
        {
            case "Enemy":
                _tower.EnemyTrigger(other.gameObject);
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                _tower.StopAllCoroutines();
                // StopCoroutine(Attack(_currentTarget));
                _currentTarget = null;
                break;
        }
    } 
    
    //debug the collider range with Gizmos with offset
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2) transform.position + _circleCollider2D.offset, _attackRange);
    }
}
