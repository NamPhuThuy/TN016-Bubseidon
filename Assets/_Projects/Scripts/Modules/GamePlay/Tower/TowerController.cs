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
    [SerializeField] private bool isExplosive = false;
    [Header("GamePlay Information")]
    [SerializeField] private ProjectileController _projectilePrefab;
    [SerializeField] private ExplodeController _explodePrefab;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private Transform _transform;
    [SerializeField] private HPBarController _hpBar;
    [SerializeField] private Animator _animator;
        
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GamePlayManager.Instance.AddTower(this);

        _transform = transform;
    }
    
    public void ResetData()
    {
        _damage = 5f;
        _attackInterval = 1f;
    }
    
    private void OnDestroy()
    {
        GamePlayManager.Instance.RemoveTower(this);
    }
    
    public void EnemyTrigger(GameObject enemy)
    {
        StartCoroutine(Attack(enemy.GetComponent<EnemyController>()));
    }

    private IEnumerator Attack(EnemyController currentTarget)
    {
        while (currentTarget != null)
        {
            if(isExplosive)
            {
                var projectile = Instantiate(_explodePrefab, transform.position, Quaternion.identity);
                projectile.damage = _damage;
                projectile.Target = currentTarget;
            }
            else
            {
                var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
                projectile.damage = _damage;
                projectile.Target = currentTarget;
            }

            yield return new WaitForSeconds(_attackInterval);
        }
    }
    
    public bool isBeingPicked { get; set; }
    
    public void TakeDamage(float damage)
    {
        _hpBar.TakeDamage(damage/_health);
        _health -= damage;
        if (_health <= 0)
        {
            StartCoroutine(Dead());
        }
    }
    IEnumerator Dead()
    {
        _animator.Play("broke");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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
