using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeController : MonoBehaviour
{
    [SerializeField] private TowerController _tower;
    [SerializeField] private float _attackRange = 1.5f;
    
    [SerializeField] private CircleCollider2D _circleCollider2D;

    private Transform _transform;
    void Start()
    {
        _tower = GetComponentInParent<TowerController>();
        _circleCollider2D.radius = _attackRange;
        _transform = transform;
    }
    
   
    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                _tower.StopAllCoroutines();
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
