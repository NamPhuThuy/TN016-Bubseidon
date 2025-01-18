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
    [SerializeField] private GameObject _selector;//The cursor
    private Camera _mainCamera;
    [SerializeField] private GameObject _putTile;
    
    [Header("Stats")]
    public float sqrRadiusToPick = 4f;//The radius to pick the tower
    public Vector3 mousePos;
    public Vector3Int positionInt;//The position of the tilemap to put on
    [SerializeField] private BoundsInt _bounds;

    [SerializeField] private GameObject _currentPickupObject;
    [SerializeField] private Transform _hand;
    [SerializeField] private SoapController _soapDisplay;
    
    [Header("Audios")]
    [SerializeField] private AudioClip _makeBubbleSound;

    private bool _onHand = false;
    void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _bounds.xMin = -9;
        _bounds.xMax = 9;
        _bounds.yMin = -5;
        _bounds.yMax = 5;
    }

    private void Update()
    {
        mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        positionInt = GamePlayManager.Instance._map.WorldToCell(mousePos);

        if (_onHand)
        {
            if (_currentPickupObject == null)
            {
                _onHand = false;
                _soapDisplay.PickUp(false);
            }
            else
            {
                float angle = MouseAngle();
                Vector3Int pos = GetPutTilePosition(angle);
                _putTile.transform.position = GamePlayManager.Instance._map.GetCellCenterWorld(pos);
            }
        }
        
        if (Input.GetMouseButtonDown(0) && !_soapDisplay.Alert && _currentPickupObject == null)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            /*Debug.LogError($"TNam - hit {hit.collider.gameObject.name}");*/
            if (hit)
            {
                if(Vector2.SqrMagnitude(hit.transform.position - gameObject.transform.position) < sqrRadiusToPick)
                {
                    if (hit.collider.TryGetComponent<IPickupable>(out IPickupable component))
                    {
                        _onHand = true;
                        PickUpObject(hit.collider.gameObject);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && _currentPickupObject != null)
        {
            _onHand = false;
            DropObject(GamePlayManager.Instance._map.WorldToCell(_putTile.transform.position));
        }
 
        if(_selector !=null)
        {
            _selector.transform.position= positionInt;
        }
    }

    private float MouseAngle()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 playerPosition = transform.position;
        
        Vector3 direction = mousePosition - playerPosition;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Debug.Log($"Angle: {angle}"); 
        
        return angle;
    }

    private Vector3Int GetPutTilePosition(float angle)
    {
        Vector3Int pos = GamePlayManager.Instance._map.WorldToCell(transform.position);
        if(angle > -22.5f && angle <= 22.5f)
        {
            pos += new Vector3Int(1, 0, 0);
        }
        else if(angle > 22.5f && angle <= 67.5f)
        {
            pos += new Vector3Int(1, 1, 0);
        }
        else if (angle > 67.5f && angle <= 112.5f)
        {
            pos += new Vector3Int(0, 1, 0);
        }
        else if (angle > 112.5f && angle <= 157.5f)
        {
            pos += new Vector3Int(-1, 1, 0);
        }
        else if (angle > 157.5f || angle <= -157.5f)
        {
            pos += new Vector3Int(-1, 0, 0);
        }
        else if (angle > -157.5f && angle <= -112.5f)
        {
            pos += new Vector3Int(-1, -1, 0);
        }
        else if (angle > -112.5f && angle <= -67.5f)
        {
            pos += new Vector3Int(0, -1, 0);
        }
        else if (angle > -67.5f && angle <= -22.5f)
        {
            pos += new Vector3Int(1, -1, 0);
        }

        if(pos.x < _bounds.xMin) pos.x = _bounds.xMin;
        else if(pos.x >= _bounds.xMax - 1) pos.x = _bounds.xMax - 1;
        if(pos.y < _bounds.yMin) pos.y = _bounds.yMin;
        else if(pos.y >= _bounds.yMax - 1) pos.y = _bounds.yMax - 1;
        
        return pos;
    }

    private void PickUpObject(GameObject hit)
    {
        _putTile.SetActive(true);
        
        _soapDisplay.gameObject.SetActive(true);
        _soapDisplay.PickUp(true);

        _currentPickupObject = hit;
        _currentPickupObject.transform.SetParent(gameObject.transform);
        _currentPickupObject.transform.position = _hand.position;
        _currentPickupObject.GetComponent<Collider2D>().excludeLayers = LayerMaskHelper.Everything();
        
        //play sound
        AudioManager.Instance.PlaySfx(_makeBubbleSound);
        
        switch (_currentPickupObject.tag)
        {
            case "Enemy":
                _currentPickupObject.GetComponent<EnemyController>().StopMoving();
                break;
        }
    }

    public void DropObject(Vector3Int pos)
    {
        _putTile.SetActive(false);
        
        _currentPickupObject.transform.SetParent(null);
        _currentPickupObject.transform.position = GamePlayManager.Instance._map.GetCellCenterWorld(pos);
        _currentPickupObject.GetComponent<Collider2D>().excludeLayers = LayerMaskHelper.Nothing();
                
        switch (_currentPickupObject.tag)
        {
            case "Enemy":
                _currentPickupObject.GetComponent<EnemyController>().BackToPath(GamePlayManager.Instance._map.WorldToCell(_currentPickupObject.transform.position));
                break;
        }
                
        _soapDisplay.PickUp(false);
        _currentPickupObject = null;
    }
    
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