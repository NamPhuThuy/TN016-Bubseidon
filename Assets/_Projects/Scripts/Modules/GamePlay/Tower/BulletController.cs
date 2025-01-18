using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private EnemyController _target;
    [SerializeField] private float _speed;
    [SerializeField]private Transform[] children;
    [SerializeField]private bool isParent;
    [SerializeField]private float _deadTime;
    [SerializeField]private float inaccuracy;
    [SerializeField]private Camera mCam;
    public float damage;
    private Rigidbody2D rb;
    
    public EnemyController Target
    {
        get => _target;
        set => _target = value;
    }
    void Awake()
    {
        if(isParent)
            foreach (Transform child in children) 
            {
                child.SetParent(null, true);
            }
    }
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        mCam=GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 mousePos= _target.transform.position;
        Vector3 dir=mousePos - transform.position;
        Vector3 rot= transform.position- mousePos;
        float baseAngle = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
        float spreadAngle = baseAngle + inaccuracy;
        Vector3 spreadDirection = new Vector3(Mathf.Cos(spreadAngle * Mathf.Deg2Rad), Mathf.Sin(spreadAngle*Mathf.Deg2Rad), 0);//Math
        rb.velocity = spreadDirection * _speed;
        transform.rotation = Quaternion.Euler(0f,0f,spreadAngle+90);
        Destroy(gameObject,_deadTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Enemy")
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

