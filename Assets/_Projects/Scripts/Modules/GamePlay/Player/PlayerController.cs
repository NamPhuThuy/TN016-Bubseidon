using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMoveable
{
    [Header("Components")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Stats")]
    [SerializeField] private Vector2 _direction;

    [Header("Audio")] 
    [SerializeField] private AudioClip _runSound;
    [SerializeField] private AudioSource _audioSource;
    private bool _isRunning = false;

    
    /// <summary>
    /// {"Idle", "Run"}
    /// </summary>
    [Header("Animations")] 
    [SerializeField] private List<string> _animPrefixList = new List<string>(){"Idle", "Run"};
    
    /// <summary>
    /// {"Side", "Front", "Back"}
    /// </summary>
    [SerializeField] private List<string> _animSuffixList = new List<string>(){"Side", "Front", "Back"};
    private string _animPrefix = "Idle";
    private string _animSuffix = "Side";
    
    
    [SerializeField] private BoundsInt _bounds;
    
    #region MonoBehaviour Methods
    void Start()
    {
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        MoveSpeed = 5f;

        //bounds for this game
        _bounds.xMin = -9;
        _bounds.xMax = 9;
        _bounds.yMin = -5;
        _bounds.yMax = 5;
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandle();
        DirectionHandle();
        AnimationHandle();

        if (MoveDirection.sqrMagnitude > 0f)
        {
            if (!_isRunning)
            {
                _audioSource.Play();
                _isRunning = true;
            }
        }
        else
        {
            if (_isRunning)
            {
                _audioSource.Stop();
                _isRunning = false;
            }
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
    #endregion
    
    

    #region IMovable Implementation

    public float MoveSpeed { get; set; }
    public Vector2 MoveDirection { get; set; }
    public void MovementHandle()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
        MoveDirection = _direction;
        
        // Calculate the amount of position-changing (deltaPosition)
        Vector3 deltaPosition = (Vector3)MoveDirection * (MoveSpeed * Time.deltaTime);

        // Check if the "deltaPosition" bring the player cross the _bounds
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
    }

    public void DirectionHandle()
    {
        _spriteRenderer.flipX = MoveDirection.x < 0f;
    }

    public void AnimationHandle()
    {
        if (MoveDirection.sqrMagnitude > 0f)
            _animPrefix = _animPrefixList[1];
        else
            _animPrefix = _animPrefixList[0];
        
        if (MoveDirection.x != 0)
            _animSuffix = _animSuffixList[0];
        else if (MoveDirection.y > 0)
            _animSuffix = _animSuffixList[2];
        else if (MoveDirection.y < 0)
            _animSuffix = _animSuffixList[1];
        
        _animator.Play(_animPrefix + _animSuffix);
        
    }
    #endregion
}
