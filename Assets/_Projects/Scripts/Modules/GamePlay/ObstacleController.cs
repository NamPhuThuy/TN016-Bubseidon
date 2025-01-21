using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class ObstacleController : MonoBehaviour, IPickupable, IDamageable
{
    [SerializeField] private HPBarController _hpBar;
    
    [Header("Components")]
    [SerializeField] private Transform _transform;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider2D;
    
    [Header("Audios")]
    [SerializeField] private AudioClip _destroySound;


    #region MonoBehaviour Methods

    private void Start()
    {
        Health = 100f;
        
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _transform = transform;
    }

    #endregion

    #region IPickupable Implementation

    public bool isBeingPicked { get; set; }
    public void OnPickUp(Transform hand)
    {
        _transform.parent = hand;
        _transform.position = hand.position;
        _collider2D.excludeLayers = LayerMaskHelper.Everything();
    }

    public void OnDropDown(Vector3Int pos)
    {
        _transform.parent = null;
        _transform.position = GamePlayManager.Instance._map.GetCellCenterWorld(pos);
        _collider2D.excludeLayers = LayerMaskHelper.Nothing();
    }

    #endregion


    #region IDamageable Implementation

    public float Health { get; set; }
    public bool IsDead { get; }

    public void TakeDamage(float amount)
    {
        _hpBar.TakeDamage(amount/Health);
        Health -= amount;
        if (Health <= 0f)
        {
            OnDead();
        }
    }
    
    public void OnDead()
    {
        _collider2D.enabled = false;
        AudioManager.Instance.PlaySfx(_destroySound);
        _animator.Play("destroy");
        Destroy(gameObject, _destroySound.length);
    }

    #endregion
}
