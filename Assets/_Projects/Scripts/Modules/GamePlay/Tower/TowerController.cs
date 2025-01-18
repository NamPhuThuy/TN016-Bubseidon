using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour, IPickupable
{
    [Header("Stats")] 
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _attackInterval = 1f;
    private float nextFireTime;
    
    [Header("GamePlay Information")]
    [SerializeField] private ProjectileController _projectilePrefab;
    
    [SerializeField] private BoxCollider2D _selfCollider2D;
    [SerializeField] private CircleCollider2D _rangeCollider2D;
    [SerializeField] private Transform _transform;
    [SerializeField] private HPBarController _hpBar;

    [SerializeField] private EnemyController _currentTarget;
    // [SerializeField] private List<EnemyController> enemiesInRange = new List<EnemyController>();
        
    
    [Header("Audio")]
    [SerializeField] private AudioClip _shootAudio;
    
    #region MonoBehaviour methods

    private void Start()
    {
        _selfCollider2D = GetComponent<BoxCollider2D>();
        _rangeCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        GamePlayManager.Instance.AddTower(this);

        _transform = transform;
        _currentTarget = null;
    }
    
    private void OnDestroy()
    {
        GamePlayManager.Instance.RemoveTower(this);
    }

    private void Update()
    {
        if (_currentTarget ==null)
        {
            // enemiesInRange.Remove(_currentTarget);
            _currentTarget = FindClosestEnemyInRange();
        }
        else
        {
            if (Vector2.Distance(_currentTarget.transform.position, transform.position) > _rangeCollider2D.radius)
            {
                _currentTarget = null;
                return;
            }
            Attack(_currentTarget);
        }
    }

    private EnemyController FindClosestEnemyInRange()
    {
        if (GamePlayManager.Instance.EnemyList.Count <= 0) return null;
        
        float minDistance = Mathf.Infinity;
        EnemyController closestEnemy = null;
        foreach (EnemyController enemy in GamePlayManager.Instance.EnemyList)
        {
            float distance = Vector2.SqrMagnitude(_transform.position - enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    #endregion
    
    public void ResetData()
    {
        _damage = 5f;
        _attackInterval = 1f;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"TNam - {other.transform.name} in tower range");
        if (other.IsTouching(_rangeCollider2D))
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponent<EnemyController>();
                // if (enemiesInRange.Contains(enemy)) return;
                //
                // enemiesInRange.Add(enemy);
            }
        }
      
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.IsTouching(_rangeCollider2D))
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponent<EnemyController>();
                // if (enemiesInRange.Contains(enemy))
                //     enemiesInRange.Remove(enemy);
            }
        }
    }


    private void Attack(EnemyController currentTarget)
    {
        Debug.Log($"Attack {currentTarget.name}");
        
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + _attackInterval;
        
        AudioManager.Instance.PlaySfx(_shootAudio);
        
        
        var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        projectile.damage = _damage;
        projectile.Target = currentTarget;
    }
    
    public bool isBeingPicked { get; set; }
    
    public void TakeDamage(float damage)
    {
        _hpBar.TakeDamage(damage/_health);
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
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
