using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour, IPickupable
{
    [Header("Stats")]
    [SerializeField] private float _moveSpeed = 1f;

    [SerializeField] private Transform _transform;

    public Vector3Int _startPos;
    public Vector3Int _endPos;

    [SerializeField] private Tilemap _tilemap;
    private List<Vector3Int> _directions;
    private Coroutine _enemyMoveCoroutine;
    

    private void Start()
    {
        /*this.OnBeingPickedEvent += StopMoving;
        OnBeingPickedEvent.Invoke();*/
        
        Debug.LogError($"Enemy Start");
        _transform.position = _tilemap.CellToWorld(_startPos);
        FindNewPath(_startPos);
    }
    
    

    private void OnEnable()
    {
        
        Debug.LogError($"On enable");
        GamePlayManager.Instance.AddEnemy(this);
        
        //Retrieve 
        _tilemap = GamePlayManager.Instance._map;
        _transform = transform;
        
        
        _directions = new List<Vector3Int>(){ new Vector3Int(0, 1, 0), 
            new Vector3Int(0, -1, 0), 
            new Vector3Int(1, 0, 0), 
            new Vector3Int(-1, 0, 0)};
    }

    private void OnDestroy()
    {
        GamePlayManager.Instance.RemoveEnemy(this);
    }
    
    //co the toi uu hon bang A*
    private List<Vector3Int> BFS(Vector3Int start, Vector3Int end)
    {
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> trace = new Dictionary<Vector3Int, Vector3Int>();
        
        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            if (current == end)
            {
                List<Vector3Int> path = new List<Vector3Int>();
                while (current != start)
                {
                    path.Add(current);
                    current = trace[current];
                }
                path.Add(start);
                path.Reverse();
                return path;
            }

            foreach (var dir in _directions)
            {
                Vector3Int next = current + dir;

                if (!visited.Contains(next) && _tilemap.HasTile(next))
                {
                    if (_tilemap.GetTile(next).name.ToLower().Contains("dirt"))
                    {
                        visited.Add(next);
                        queue.Enqueue(next);
                        trace[next] = current;
                    }
                }
            }
        }

        return null;
    }
    
    private IEnumerator MoveAlongPath(Queue<Vector3Int> pathQueue)
    {
        while (pathQueue.Count > 0)
        {
            Vector3Int nextTile = pathQueue.Dequeue();
            
            Vector3 targetPosition = _tilemap.GetCellCenterWorld(nextTile);
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
                yield return null;
            }
            
            transform.position = targetPosition;

            yield return null;
        }

        Debug.Log("Ve dich");
    }
    
    public void BackToPath(Vector3Int playerPosition)
    {
        _transform.position = _tilemap.GetCellCenterWorld(playerPosition);
        
        if (_tilemap.GetTile(playerPosition).name.ToLower().Contains("dirt"))
        {
            FindNewPath(playerPosition);
            return;
        }
        
        List<Vector3Int> checkTiles = new List<Vector3Int>();
        BoundsInt bounds = _tilemap.cellBounds;

        Vector3Int playerPos = playerPosition;
        while (playerPos.x > bounds.xMin)
        {
            Vector3Int checkTile = new Vector3Int(playerPos.x - 1, playerPos.y, playerPos.z);
            if(checkTile == _endPos) continue;
            if (_tilemap.GetTile(checkTile) == null) break;
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains("dirt"))
            {
                checkTiles.Add(checkTile);
                break;
            }
            playerPos = checkTile;
        }
        playerPos = playerPosition;
        while (playerPos.x < bounds.xMax)
        {
            Vector3Int checkTile = new Vector3Int(playerPos.x + 1, playerPos.y, playerPos.z);
            if(checkTile == _endPos) continue;
            if (_tilemap.GetTile(checkTile) == null) break;
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains("dirt"))
            {
                checkTiles.Add(checkTile);
                break;
            }

            playerPos = checkTile;
        }
        playerPos = playerPosition;
        while (playerPos.y > bounds.yMin)
        {
            Vector3Int checkTile = new Vector3Int(playerPos.x, playerPos.y - 1, playerPos.z);
            if(checkTile == _endPos) continue;
            if (_tilemap.GetTile(checkTile) == null) break;
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains("dirt"))
            {
                checkTiles.Add(checkTile);
                break;
            }
            playerPos = checkTile;
        }
        playerPos = playerPosition;
        while (playerPos.y < bounds.yMax)
        {
            Vector3Int checkTile = new Vector3Int(playerPos.x, playerPos.y + 1, playerPos.z);
            if(checkTile == _endPos) continue;
            if (_tilemap.GetTile(checkTile) == null) break;
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains("dirt"))
            {
                checkTiles.Add(checkTile);
                break;
            }
            playerPos = checkTile;
        }

        Vector3Int chosenTile = default;
        float distance = float.MaxValue;
        foreach (var tile in checkTiles)
        {
            if (Vector3.Distance(tile, _endPos) < distance)
            {
                distance = Vector3.Distance(tile, playerPosition);
                chosenTile = tile;
            }
        }
        
        StartCoroutine(MoveFromAToB(chosenTile));
    }
    
    IEnumerator MoveFromAToB(Vector3Int end)
    {
        Vector3 endWorldPos = _tilemap.GetCellCenterWorld(end);
        
        while (Vector3.Distance(_transform.position, endWorldPos) > 0.01f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, endWorldPos, _moveSpeed * Time.deltaTime);
            
            yield return null;
        }
        
        _transform.position = endWorldPos;
        
        FindNewPath(_tilemap.WorldToCell(end));
    }
    
    public void FindNewPath(Vector3Int startPos)
    {
        List<Vector3Int> path = BFS(startPos, _endPos);
        Queue<Vector3Int> pathQueue = new Queue<Vector3Int>();
        
        if (path != null)
        {
            foreach (var pos in path)
            {
                pathQueue.Enqueue(pos);
            }
            
            _enemyMoveCoroutine = StartCoroutine(MoveAlongPath(new Queue<Vector3Int>(pathQueue)));
        }
        else
        {
            Debug.Log("Khong co duong di");
        }
    }
    
    public void StopMoving()
    {
        // StopCoroutine(_enemyMoveCoroutine);
        StopAllCoroutines();
    }

    public bool isBeingPicked { get; set; }
    public event Action OnBeingPickedEvent;
    public event Action OnBeingDropEvent;
}
