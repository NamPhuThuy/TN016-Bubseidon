using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour, IPickupable
{
    [Header("Stats")] 
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _attackInterval = 1f;
    
    [Header("GamePlay Information")]
    [SerializeField] private EnemyController _currentTarget;
    [SerializeField] private ProjectileController _projectilePrefab;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private Transform _transform;
        
    
    private void OnEnable()
    {
        GamePlayManager.Instance.AddTower(this);

        _transform = transform;
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.radius = _attackRange;
    }
    
    public void ResetData()
    {
        _attackRange = 1.5f;
        _damage = 5f;
        _attackInterval = 1f;
    }
    
    private void OnDestroy()
    {
        GamePlayManager.Instance.RemoveTower(this);
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
        Debug.Log($"Collide with {other.transform.name}");
        switch (other.tag)
        {
            case "Enemy":
                _currentTarget = other.gameObject.GetComponent<EnemyController>();
                StartCoroutine(Attack(_currentTarget));
                break;
        }
    }

    private IEnumerator Attack(EnemyController currentTarget)
    {
        while (currentTarget != null)
        {
            var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            projectile.damage = _damage;
            projectile.Target = currentTarget;
            yield return new WaitForSeconds(_attackInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                StopAllCoroutines();
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

    public bool isBeingPicked { get; set; }
}

//inspector code
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(TowerController))]
public class TowerControllerEditor : UnityEditor.Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TowerController towerController = (TowerController)target;

        if (GUILayout.Button("Reset"))
        {
            towerController.ResetData();
        }
    }
    private void OnSceneGUI()
    {
        /*TowerController towerController = (TowerController) target;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(towerController.transform.position, Vector3.back, towerController._attackRange);*/
    }
}
#endif
