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
    
    [Header("Stats")]
    public float sqrRadiusToPick = 49f;//The radius to pick the tower
    public Vector3 mousePos;
    public Vector3Int positionInt;//The position of the tilemap to put on

    [SerializeField] private GameObject _currentPickupObject;
    [SerializeField] private Transform _hand;
    [SerializeField] private SoapController _soapDisplay;

    private bool _onHand = false;
    void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        positionInt = GamePlayManager.Instance._map.WorldToCell(mousePos);

        if (_onHand && _currentPickupObject == null)
        {
            _onHand = false;
            _soapDisplay.PickUp(false);
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
            if(Vector2.SqrMagnitude(positionInt - gameObject.transform.position) < sqrRadiusToPick)
            {
                _onHand = false;
                DropObject(positionInt);
            }
        }
 
        if(_selector !=null)
        {
            _selector.transform.position= positionInt;
        }
    }

    private void PickUpObject(GameObject hit)
    {
        _soapDisplay.gameObject.SetActive(true);
        _soapDisplay.PickUp(true);

        _currentPickupObject = hit;
        _currentPickupObject.transform.SetParent(gameObject.transform);
        _currentPickupObject.transform.position = _hand.position;
        _currentPickupObject.GetComponent<Collider2D>().excludeLayers = LayerMaskHelper.Everything();
        
        switch (_currentPickupObject.tag)
        {
            case "Enemy":
                _currentPickupObject.GetComponent<EnemyController>().StopMoving();
                break;
        }
    }

    public void DropObject(Vector3Int pos)
    {
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