using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        positionInt = new Vector3Int(Mathf.FloorToInt(mousePos.x + 0.5f), Mathf.FloorToInt(mousePos.y + 0.5f), 0);

        if (Input.GetMouseButtonDown(0))
        {
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            Debug.LogError($"TNam - hit {hit.collider.gameObject.name}");
            if (hit)
            {
                if(Vector2.SqrMagnitude(hit.transform.position - gameObject.transform.position) < sqrRadiusToPick)
                {
                    if (hit.collider.TryGetComponent<IPickupable>(out IPickupable component))
                    {
                        _currentPickupObject = hit.collider.gameObject;
                        _currentPickupObject.transform.SetParent(gameObject.transform);
                        _currentPickupObject.transform.position = _hand.position;
                        switch (_currentPickupObject.tag)
                        {
                            case "Enemy":
                                _currentPickupObject.GetComponent<EnemyController>().StopMoving();
                                break;
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && _currentPickupObject != null)
        {
            if(Vector2.SqrMagnitude(positionInt - gameObject.transform.position) < sqrRadiusToPick)
            {
                _currentPickupObject.transform.SetParent(null);
                _currentPickupObject.transform.position = positionInt;
                
                switch (_currentPickupObject.tag)
                {
                    case "Enemy":
                        _currentPickupObject.GetComponent<EnemyController>().BackToPath(GamePlayManager.Instance._map.WorldToCell(_currentPickupObject.transform.position));
                        break;
                }
                
                _currentPickupObject = null;
            }
        }
 
        if(_selector !=null)
        {
            _selector.transform.position= positionInt;
        }
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