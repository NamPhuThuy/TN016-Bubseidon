using UnityEngine;
using UnityEngine.Tilemaps;

public class DragEnemy : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    
    [SerializeField] private EnemyController _pathFinding;
    
    private bool _isDragging = false;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDragging();
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            _pathFinding.StopMoving();
            DragObject();
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            DropObject();
        }
    }
    
    private void StartDragging()
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        
        Collider2D collider = Physics2D.OverlapPoint(mouseWorldPosition);
        if (collider != null && collider.gameObject == this.gameObject)
        {
            _isDragging = true;
        }
    }
    
    private void DragObject()
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        
        transform.position = mouseWorldPosition;
    }
    
    private void DropObject()
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        
        Vector3Int cellPosition = _tilemap.WorldToCell(mouseWorldPosition);
        Debug.Log("Cell position: " + cellPosition);
        
        Vector3 cellCenterPosition = _tilemap.GetCellCenterWorld(cellPosition);
        
        transform.position = cellCenterPosition;

        _pathFinding.BackToPath(_tilemap.WorldToCell(transform.position));
        
        _isDragging = false;
    }
}