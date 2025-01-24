using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour, IPickupable, IAttackable, IDamageable
{
    [Header("Stats")] 
    [SerializeField] private float _health = 100f;
    
    [Header("Components")]
    [SerializeField] private BoxCollider2D _selfCollider2D;
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

    [Header("Animations")] 
    private readonly string _ruinedAnimString = "Ruined";
    private readonly string _idleAnimString = "Idle";
    
    #region MonoBehaviour methods

    private void Start()
    {
        //Attributes
        AttackCoolDown = 0.1f;
        AttackTimer = Time.time;
        AttackRange = 3f;
        AttackDamage = 1.2f;
        
        //Component References
        _selfCollider2D = GetComponent<BoxCollider2D>();
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
    #endregion
    
    public void ResetData()
    {
        AttackDamage = 5f;
        AttackCoolDown = 0.5f;
    }

    #region IPickupable Implementation

    public bool isBeingPicked { get; set; }
    public void OnPickUp(Transform hand)
    {
        _transform.parent = hand;
        _transform.position = hand.position;
        _selfCollider2D.excludeLayers = LayerMaskHelper.Everything();
    }

    public void OnDropDown(Vector3Int pos)
    {
        _transform.parent = null;
        _transform.position = GamePlayManager.Instance._map.GetCellCenterWorld(pos);
        _selfCollider2D.excludeLayers = LayerMaskHelper.Nothing();
    }

    #endregion

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
        _animator.Play(_ruinedAnimString);
        AudioManager.Instance.PlaySfx(_destroyAudio);
        Destroy(gameObject, 1.2f);
    }
    #endregion
    
    #region IAttackable Implementation

    public float AttackDamage { get; set; }
    public float AttackCoolDown { get; set; }
    public float AttackTimer { get; set; }
    public float AttackRange { get; set; }
    public IDamageable Target { get; set; }
    public void Attack()
    {
        if (Time.time < AttackTimer) return;
        AttackTimer = Time.time + AttackCoolDown;
        
        AudioManager.Instance.PlaySfx(_shootAudio);
        
        var projectile = Instantiate(_projectilePrefab, _pivot.transform.position, Quaternion.identity);
        projectile.AttackDamage = AttackDamage;
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
