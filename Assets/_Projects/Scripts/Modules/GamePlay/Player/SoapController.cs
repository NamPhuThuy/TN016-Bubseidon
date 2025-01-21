using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapController : MonoBehaviour
{
    [Header("Components")]
    
    [SerializeField] private GameObject _bubble;
    [SerializeField] private Transform _soapBar;
    [SerializeField] private Transform _limitBar;
    [SerializeField] private float _soapDecreaseSpeed = 0.5f;
    [SerializeField] private float _soapIncreaseSpeed = 0.3f;
    [SerializeField]private Animator _animator;
    private Transform _transform;

    private float _maxSoap;
    private float _currentSoap;
    private float _limitSoap;
    private bool _alert = false;
    public bool Alert
    {
        get => _alert;
    }
    [SerializeField] private bool _pickingUp = true;

    void OnEnable()
    {
        _animator.Play("Idle");
        _pickingUp = true;
        _bubble.SetActive(true);
    }
    
    void OnDisable()
    {
        _pickingUp = false;
        _bubble.SetActive(false);
    }
    
    void Start()
    {
        _transform = transform;
        _maxSoap = _soapBar.GetComponent<SpriteRenderer>().bounds.size.x;
        _currentSoap = _maxSoap;
        _limitSoap = _maxSoap * (0.5f - Mathf.Abs(_limitBar.localPosition.x));
    }

    void Update()
    {
        if (_pickingUp)
        {
            if (_currentSoap > 0)
            {
                float decStep = _soapDecreaseSpeed * Time.deltaTime;
                _currentSoap -= decStep;
                _soapBar.position -= new Vector3(decStep, 0, 0);

                if (_currentSoap <= _limitSoap && _alert == false)
                {
                    _alert = true;
                    _soapBar.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else
            {
                Transform player = GamePlayManager.Instance.Player.transform;
                PlayerPickUpMechanics playerPickUp = player.GetComponent<PlayerPickUpMechanics>();
                Vector3 pos = new Vector3(_bubble.transform.position.x, player.position.y, _bubble.transform.position.z);
                playerPickUp.DropObject(GamePlayManager.Instance._map.WorldToCell(pos));
                
            }
        }
        else
        {
            if (_currentSoap < _maxSoap)
            {
                float incStep = _soapIncreaseSpeed * Time.deltaTime;
                _currentSoap += incStep;
                _soapBar.position += new Vector3(incStep, 0, 0);
                
                if (_currentSoap >= _limitSoap && _alert == true)
                {
                    _alert = false;
                    _soapBar.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator PickUp(bool check)
    {
        if (check)
        {
            _pickingUp = true;
            _bubble.SetActive(true);
        }
        else
        {
            _pickingUp = false;
            _animator.Play("ded");
            yield return new WaitForSeconds(0.8f);
            _bubble.SetActive(false);
            
        }
        yield return null;
    }
}
