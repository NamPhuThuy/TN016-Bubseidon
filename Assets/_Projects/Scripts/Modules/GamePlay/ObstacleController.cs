using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour, IPickupable
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private HPBarController _hpBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private AudioClip _destroySound;

    public bool isBeingPicked { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void TakeDamage(float damage)
    {
        _hpBar.TakeDamage(damage/_health);
        _health -= damage;
        if (_health <= 0)
        {
            
            _collider2D.enabled = false;
            AudioManager.Instance.PlaySfx(_destroySound);
            _animator.Play("destroy");
            Destroy(gameObject, 2.4f);
        }
    }
   
}
