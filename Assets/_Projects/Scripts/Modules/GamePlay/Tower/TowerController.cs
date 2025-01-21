using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour, IPickupable, IAttackable, IDamageable
{
    [Header("Stats")] 
    [SerializeField] private float _health = 100f;
    
    [Header("Components")]
    [SerializeField] private BoxCollider2D _selfCollider2D;
    [SerializeField] private CircleCollider2D _rangeCollider2D;
    [SerializeField] private Transform _transform;
    [SerializeField] private HPBarController _hpBar;
    [SerializeField] private Animator _animator;

    
    [Header("GamePlay Information")]
    [SerializeField] private ProjectileController _projectilePrefab;
    [SerializeField] private EnemyController _currentTarget;
    [SerializeField] private GameObject _pivot; // for check range
    [SerializeField] private List<ProjectileController> _projectilePool = new List<ProjectileController>(30);
        
    
    [Header("Audio")]
    [SerializeField] private AudioClip _shootAudio;
    [SerializeField] private AudioClip _destroyAudio;
    
    #region MonoBehaviour methods

    private void Start()
    {
        //Attributes
        AttackInterval = 0.1f;
        AttackTimer = Time.time;
        AttackRange = 3f;
        Damage = 1.2f;
        
        //Component References
        _selfCollider2D = GetComponent<BoxCollider2D>();
        _rangeCollider2D = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _transform = transform;
        
        _currentTarget = null;
    }
    
    private void OnEnable()
    {
        GamePlayManager.Instance.AddTower(this);
        
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
            if ((Vector2.Distance(_currentTarget.transform.position, transform.position) > AttackRange) || (_currentTarget.IsDead))
            {
                _currentTarget = null;
                return;
            }
            
            Attack();
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

        if (minDistance > AttackRange * AttackRange)
            return null;

        return closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
    #endregion
    
    public void ResetData()
    {
        Damage = 5f;
        AttackInterval = 0.5f;
    }
    
    public bool isBeingPicked { get; set; }

    #region IDamageable Implementation
    public float Health { get; set; }
    public bool IsDead { get; }
    public void TakeDamage(float damage)
    {
        _hpBar.TakeDamage(damage/_health);
        _health -= damage;
        if (_health <= 0)
        {
            OnDead();
        }
    }

    
    public void OnDead()
    {
        _animator.Play("broke");
        AudioManager.Instance.PlaySfx(_destroyAudio);
        Destroy(gameObject, 1.2f);
    }
    #endregion

   

    #region IAttackable Implementation

    public float Damage { get; set; }
    public float AttackInterval { get; set; }
    public float AttackTimer { get; set; }
    public float AttackRange { get; set; }
    public IDamageable Target { get; set; }
    public void Attack()
    {
        if (Time.time < AttackTimer) return;
        AttackTimer = Time.time + AttackInterval;
        
        AudioManager.Instance.PlaySfx(_shootAudio);
        
        var projectile = Instantiate(_projectilePrefab, _pivot.transform.position, Quaternion.identity);
        projectile.Damage = Damage;
        projectile.TargetTransform = _currentTarget.transform;
    }

    #endregion
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
