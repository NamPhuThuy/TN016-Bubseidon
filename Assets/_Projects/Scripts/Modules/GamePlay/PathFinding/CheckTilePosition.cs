using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckTilePosition : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            Vector3Int cellPosition = _tilemap.WorldToCell(worldPosition);
            
            Debug.Log("Tile clicked at cell position: " + cellPosition);
        }
    }
}
