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
    [Header("Area Damage")]
    [SerializeField] private bool multipleEnemy;
    private List<Collider2D> enemiesInRange = new List<Collider2D>();


    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            if(multipleEnemy)
            {
                AreaDamage(); 
                Destroy(gameObject,1f);           
            }
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
        if(_speed > 0)
            HandleDirection();
    }
    private void HandleDirection()
    {
        Vector3 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            enemiesInRange.Add(collision);
            Debug.Log($"Enemy entered: {collision.gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision);
            Debug.Log($"Enemy exited: {collision.gameObject.name}");
        }
    }
    public void AreaDamage()
    {
        foreach (var enemyCollider in enemiesInRange)
        {
            if (enemyCollider != null)
            {
                EnemyController enemy = enemyCollider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
