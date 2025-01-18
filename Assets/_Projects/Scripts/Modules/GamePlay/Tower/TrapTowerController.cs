using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTowerController : MonoBehaviour
{
    [SerializeField] private float _damage = 5f;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(_damage);
            other.GetComponent<EnemyController>().setSpeed(0.5f);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().setSpeed(-0.5f);
        }
    }
}
