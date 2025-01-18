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
    [SerializeField] private BoundsInt _bounds;
    
    void Start()
    {
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();

        //bounds for this game
        _bounds.xMin = -9;
        _bounds.xMax = 9;
        _bounds.yMin = -5;
        _bounds.yMax = 5;
    }

    // Update is called once per frame
    void Update()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
        
        // Tính toán lượng thay đổi vị trí (deltaPosition)
        Vector3 deltaPosition = (Vector3)_direction * (_moveSpeed * Time.deltaTime);

        // Kiểm tra nếu deltaPosition sẽ đưa nhân vật vượt quá _bounds
        if (transform.position.x + deltaPosition.x < _bounds.xMin)
        {
            deltaPosition.x = _bounds.xMin - transform.position.x;
        }
        else if (transform.position.x + deltaPosition.x >= _bounds.xMax)
        {
            deltaPosition.x = _bounds.xMax - transform.position.x;
        }

        if (transform.position.y + deltaPosition.y < _bounds.yMin)
        {
            deltaPosition.y = _bounds.yMin - transform.position.y;
        }
        else if (transform.position.y + deltaPosition.y >= _bounds.yMax - 1)
        {
            deltaPosition.y = _bounds.yMax - 1 - transform.position.y;
        }

        // Cập nhật vị trí
        transform.Translate(deltaPosition);

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
