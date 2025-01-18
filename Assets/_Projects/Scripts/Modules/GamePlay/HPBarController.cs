using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    [SerializeField] private Transform _hpBar;
    
    private Transform _transform;

    private float _maxHP;
    private float _currentHP;
    
    void Start()
    {
        _transform = transform;
        _maxHP = _hpBar.GetComponent<SpriteRenderer>().bounds.size.x;
        _currentHP = _maxHP;
    }

    public void TakeDamage(float dmg)
    {
        float takenDmg = dmg * _currentHP;
        _currentHP -= takenDmg;
        _hpBar.position -= new Vector3(takenDmg, 0, 0);
    }
}
