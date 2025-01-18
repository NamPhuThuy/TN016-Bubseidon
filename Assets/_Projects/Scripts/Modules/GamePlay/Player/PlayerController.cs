using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Animator _animator;

    [Header("Stats")] 
    [SerializeField] private Vector2 _direction;
    
    void Start()
    {
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
        
        
        //handle movement
        transform.Translate(_direction * (_moveSpeed * Time.deltaTime));
        
        //handle direction
        if (_direction.x > 0)
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (_direction.x < 0)
        {
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        //handle animations
        if (_direction.x != 0 ||  _direction.y != 0)
        {
            _animator.Play("run");
        }
        else
        {
            _animator.Play("idle");
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Coin"))
        {
            DataManager.Instance.PlayerData.coin++;
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
        }
        
    }
}
