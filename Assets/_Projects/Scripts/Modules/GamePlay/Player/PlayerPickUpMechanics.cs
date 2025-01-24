using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class PlayerPickUpMechanics : MonoBehaviour
{
    [Header("Tilemap")]
    [SerializeField] private Tilemap _interact;//If need to interact with the tilemap, add it
    private Camera _mainCamera;
    [SerializeField] private GameObject _putTile;

    [Header("Components")] 
    [SerializeField] private Transform _transform;
    
    [Header("Stats")]
    [SerializeField] private float _radiusToPick;//The radius to pick the tower
    public Vector3 mousePos;
    public Vector3Int mousePositionInt;//The position of the tilemap to put on
    [SerializeField] private BoundsInt _bounds;
    
    [SerializeField] private float _mpIncreaseSpeed;
    [SerializeField] private float _mpDecreaseSpeed;
    [SerializeField] private float _maxMP;
    private Coroutine _mpCoroutine;

    
    // [SerializeField] private SoapController _soapDisplay;
    
    [Header("Audios")]
    [SerializeField] private AudioClip _makeBubbleSound;
    [SerializeField] private AudioClip _bubblePopSound;

    [Header("Pickup Mechanics")] 
    [SerializeField] private GameObject _splashWater;
    [SerializeField] private GameObject _currentPickupObject;
    [SerializeField] private Transform _hand;
    [SerializeField] private BubbleController _bubbleController;

    #region MonoBehaviour Methods

    void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _bounds.xMin = -9;
        _bounds.xMax = 9;
        _bounds.yMin = -5;
        _bounds.yMax = 5;
        
        //Retrieve Values
        _mpDecreaseSpeed = DataManager.Instance.PlayerData.mpDecreaseSpeed;
        _mpIncreaseSpeed = DataManager.Instance.PlayerData.mpIncreaseSpeed;
        _maxMP = DataManager.Instance.PlayerData.maxMP;
        _radiusToPick = DataManager.Instance.PlayerData.radiusToPick;
        
        //Retrieve Components
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePositionInt = GamePlayManager.Instance._map.WorldToCell(mousePos);

        if (_currentPickupObject)
        {
            float angle = MouseAngle();
            Vector3Int pos = GetPutTilePosition(angle);
            _putTile.transform.position = GamePlayManager.Instance._map.GetCellCenterWorld(pos);
        }
        
        if (Input.GetMouseButtonDown(0) && _currentPickupObject == null)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {
                if(Vector2.Distance(hit.transform.position, _transform.position) < _radiusToPick)
                {
                    if (hit.collider.GetComponent<IPickupable>() != null)
                        PickUpObject(hit.collider.gameObject);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && _currentPickupObject != null)
        {
            DropObject(GamePlayManager.Instance._map.WorldToCell(_putTile.transform.position));
        }
    }

    #endregion

    private float MouseAngle()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPosition = transform.position;
        Vector3 direction = mousePosition - playerPosition;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return angle;
    }

    private Vector3Int GetPutTilePosition(float angle)
    {
        Vector3Int pos = GamePlayManager.Instance._map.WorldToCell(transform.position);
        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),   // 0
            new Vector3Int(1, 1, 0),   // 45
            new Vector3Int(0, 1, 0),   // 90
            new Vector3Int(-1, 1, 0),  // 135
            new Vector3Int(-1, 0, 0),  // 180
            new Vector3Int(-1, -1, 0), // -135
            new Vector3Int(0, -1, 0),  // -90
            new Vector3Int(1, -1, 0)   // -45
        };

        // Determine the put tile while rotating the mouse around Player
        int index = Mathf.RoundToInt(angle / 45f) % 8;
        if (index < 0) index += 8;

        pos += directions[index]; 
        
        // Constraint it inside the bounds
        if(pos.x < _bounds.xMin) pos.x = _bounds.xMin;
        else if(pos.x >= _bounds.xMax - 1) pos.x = _bounds.xMax - 1;
        if(pos.y < _bounds.yMin) pos.y = _bounds.yMin;
        else if(pos.y >= _bounds.yMax - 1) pos.y = _bounds.yMax - 1;
        
        return pos;
    }

    private void PickUpObject(GameObject hit)
    {
        if (_mpCoroutine != null) StopCoroutine(_mpCoroutine);
        _mpCoroutine = StartCoroutine(DecreaseMPOverTime());
        _putTile.SetActive(true);
        _bubbleController.Show();

        _currentPickupObject = hit;
        _currentPickupObject.GetComponent<IPickupable>().OnPickUp(_hand);
        
        //play sound
        AudioManager.Instance.PlaySfx(_makeBubbleSound);
    }

    public void DropObject(Vector3Int pos)
    {
        _putTile.SetActive(false);
        if (_mpCoroutine != null) StopCoroutine(_mpCoroutine);
        _mpCoroutine = StartCoroutine(IncreaseMPOverTime()); // Start a new one
        StartCoroutine(_bubbleController.Hide());
        TurnOnBubblePopSFX();
        
        _currentPickupObject.GetComponent<IPickupable>().OnDropDown(pos);
        _currentPickupObject = null;
    }

    private void TurnOnBubblePopSFX()
    {
        AudioManager.Instance.PlaySfx(_bubblePopSound);
    }
    
    #region MP Changing
    
    private IEnumerator DecreaseMPOverTime()
    {
        while (true)
        {
            DataManager.Instance.PlayerData.currentMP -= (_mpDecreaseSpeed * Time.deltaTime);
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            if (DataManager.Instance.PlayerData.currentMP <= 0f)
            {
                DropObject(GamePlayManager.Instance._map.WorldToCell(_putTile.transform.position));
            }
            yield return null;
        }
    }

    private IEnumerator IncreaseMPOverTime()
    {
        while (true)
        {
            DataManager.Instance.PlayerData.currentMP += (_mpIncreaseSpeed * Time.deltaTime);
            MessageManager.Instance.SendMessage(new Message(NamMessageType.OnDataChanged));
            if (DataManager.Instance.PlayerData.currentMP >= DataManager.Instance.PlayerData.maxMP)
            {
                StopCoroutine(_mpCoroutine);
            }
            yield return null;
        }
    }
    #endregion
    
    //Test xem có trên tilemap nào ko
    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = _interact.GetTile(position);
        if(tile != null)
        {
            if(tile.name == "test_1")
            {
                return true;
            }
        }
        return false;
    }
}