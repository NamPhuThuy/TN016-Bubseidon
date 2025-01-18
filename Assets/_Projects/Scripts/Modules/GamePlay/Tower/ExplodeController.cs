using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : MonoBehaviour
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
    private bool spawned=false;
    [SerializeField]private List<Collider2D> enemiesInRange = new List<Collider2D>();
    void Update()
    {
        if (_target != null)
        {
            if(!spawned)
            {
                StartCoroutine(DelayedAreaDamage());
                spawned = true;
                Destroy(gameObject,1f);           
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator DelayedAreaDamage()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(enemiesInRange.Count);
        AreaDamage();
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            enemiesInRange.Add(collision);
            //Debug.Log($"Enemy entered: {collision.gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision);
            //Debug.Log($"Enemy exited: {collision.gameObject.name}");
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
