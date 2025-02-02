using System;
using System.Collections;
using System.Collections.Generic;
using NamPhuThuy;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyController : MonoBehaviour, IPickupable, IDamageable, IMoveable, IAttackable
{
    [Header("Stats")]
    public Vector3Int _startPos;
    public Vector3Int _endPos;
    
    [Header("Components")]
    [SerializeField] private Transform _transform;
    [SerializeField] private HPBarController _hpBar; 
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _collider2D;

    
    [Header("Movement Handle")]
    [SerializeField] private Tilemap _tilemap;
    private string _walkableTile = "maptile_4";
    private List<Vector3Int> _directions;
    private Coroutine _enemyMoveCoroutine;
    
    [Header("Combat Related")] 
    private IDamageable _currentTarget; 
    
    [Header("Die-rewards")]
    [SerializeField] private CoinController _coinController;
    private bool _isSpawnCoin = false;
    
    [Header("AnimClip name")]
    private string _runAnimString = "Run";
    private string _hurtAnimString = "Hurt";
    private string _attackAnimString = "Attack";
    private string _dieAnimString = "Die";
    
    [Header("Stats")]
    [SerializeField] private string _id;
    
    #region MonoBehaviour methods
    private void Start()
    {
        GamePlayManager.Instance.AddEnemy(this);
        LoadData();
        isBeingPicked = false;
        LoadComponent();
        DirectionHandle();
        FindNewPath(_startPos);
    }
    
    private void Update()
    {
        if (_currentTarget != null)
        {
            if (AttackTimer < Time.time)
            {
                Attack();
                AttackTimer += AttackCoolDown;
            }
        }
    }
    
    private void OnEnable()
    {
        //Retrieve Components
        _transform = transform;
        
        _directions = new List<Vector3Int>(){ new Vector3Int(0, 1, 0), 
            new Vector3Int(0, -1, 0), 
            new Vector3Int(1, 0, 0), 
            new Vector3Int(-1, 0, 0)};
    }

    private void OnDestroy()
    {
        GamePlayManager.Instance.RemoveEnemy(this);
        MessageManager.Instance.SendMessage(new Message(NamMessageType.OnEnemyDie));
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.GetComponent<IDamageable>() != null)
        {
            _currentTarget = other.transform.GetComponent<IDamageable>();
            _animator.Play(_attackAnimString);
            StopMoving();
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            _currentTarget = null;
            _animator.Play(_runAnimString);
            BackToPath(GamePlayManager.Instance._map.WorldToCell(_transform.position));
        }
    }
    #endregion

    #region Set-up

    public void Setup(Vector3Int start, Vector3Int end)
    {
        _startPos = start;
        _endPos = end;
    }
    private void LoadData()
    {
        EnemyData enemyData = DataManager.Instance.GetEnemyDataById(_id);
        Health = enemyData.hp;
        AttackCoolDown = enemyData.attackCoolDown;
        AttackDamage = enemyData.attackDamage;
        MoveSpeed = enemyData.runSpeed;
    }

    private void LoadComponent()
    {
        _tilemap = GamePlayManager.Instance._map;
        _coinController = GamePlayManager.Instance._coinController;
        _hpBar = GetComponentInChildren<HPBarController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        _transform = transform;
        _animator = GetComponent<Animator>();
    }

    #endregion

    #region Path finding

    //co the toi uu hon bang A*
    private List<Vector3Int> BFS(Vector3Int start, Vector3Int end)
    {
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>(); // keep track of visited tiles 
        Queue<Vector3Int> queue = new Queue<Vector3Int>(); // mange the BFS frontier
        Dictionary<Vector3Int, Vector3Int> trace = new Dictionary<Vector3Int, Vector3Int>(); // trace the path from the end position back to the start position
        
        // enqueue the start position and marked it as "visited"
        queue.Enqueue(start); 
        visited.Add(start);

        //while there are positions int the queue
        while (queue.Count > 0)
        {
            // dequeue the position
            Vector3Int current = queue.Dequeue();

            // if the current position is the end position
            if (current == end)
            {
                //construct the path by tracing from the end to the start using the "trace dictionary"
                List<Vector3Int> path = new List<Vector3Int>();
                while (current != start)
                {
                    path.Add(current);
                    current = trace[current];
                }
                path.Add(start);
                
                //reverse the path to get the correct order from start to end and return it 
                path.Reverse();
                return path;
            }

            //for each posible direction
            foreach (var dir in _directions)
            {
                //calculate the next position
                Vector3Int next = current + dir;

                //If the next position is not visited and has a tile 
                if (!visited.Contains(next) && _tilemap.HasTile(next))
                {
                    //If the tile-name contains "maptile_4"
                    if (_tilemap.GetTile(next).name.ToLower().Contains(_walkableTile))
                    {
                        visited.Add(next); //Mark the next position as visited
                        queue.Enqueue(next); //Enqueue the next position
                        trace[next] = current; //Record the current position as the predecessor of the next position in the "trace dictionary"
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

            MoveDirection = (targetPosition - _transform.position).normalized;
            DirectionHandle();
            
            
            while (Vector3.Distance(_transform.position, targetPosition) > 0.01f)
            {
                MovementHandle();
                yield return null;
            }
            
            _transform.position = targetPosition;

            yield return null;
        }

        DealDamageToPlayer();
    }
    
    public void BackToPath(Vector3Int playerPosition)
    {
        _transform.position = _tilemap.GetCellCenterWorld(playerPosition);
        Debug.Log(_tilemap.GetTile(playerPosition).name.ToLower());
        if (_tilemap.GetTile(playerPosition).name.ToLower().Contains(_walkableTile))
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
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains(_walkableTile))
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
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains(_walkableTile))
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
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains(_walkableTile));
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
            if (_tilemap.GetTile(checkTile).name.ToLower().Contains(_walkableTile))
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
        MoveDirection = (endWorldPos - _transform.position).normalized;
        DirectionHandle();
        
        while (Vector3.Distance(_transform.position, endWorldPos) > 0.01f)
        {
            MovementHandle();
            
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

    #endregion

    public void DealDamageToPlayer()
    {
        DataManager.Instance.CurrentHP -= AttackDamage;
        Destroy(gameObject);
    }
    
    public void StopMoving()
    {
        // StopCoroutine(_enemyMoveCoroutine);
        StopAllCoroutines();
    }

    #region IPickupable Implementation
    public bool isBeingPicked { get; set; }
    public void OnPickUp(Transform hand)
    {
        _transform.parent = hand;
        _transform.position = hand.position;
        _collider2D.excludeLayers = LayerMaskHelper.Everything();
        StopMoving();
    }

    public void OnDropDown(Vector3Int pos)
    {
        _transform.parent = null;
        _transform.position = GamePlayManager.Instance._map.GetCellCenterWorld(pos);
        _collider2D.excludeLayers = LayerMaskHelper.Nothing();
        BackToPath(GamePlayManager.Instance._map.WorldToCell(_transform.position));
    }
    #endregion

    #region IDamageable Implementations

    public float Health { get; set; }
    public bool IsDead => Health <= 0f;

    public void TakeDamage(float amount)
    {
        _hpBar.TakeDamage(amount/Health);
        // StartCoroutine(HurtChangeColor());
        if (IsDead)
            OnDead();
        Health -= amount;
    }

    IEnumerator HurtChangeColor()
    {
        // _animator.Play(_hurtAnimString);
        // yield return new WaitForEndOfFrame();
        yield return Yielders.Get(0.1f);
    }
    
    public void OnDead()
    {
        MoveSpeed = 0f;
        _animator.Play(_dieAnimString);
        if (!_isSpawnCoin)
        {
            Instantiate(_coinController, _transform.position, Quaternion.identity);
            _isSpawnCoin = true;
        }
        Destroy(gameObject, 1f);
    }

    #endregion
    
    #region IMoveable Implementations

    public float MoveSpeed { get; set; }
    public Vector2 MoveDirection { get; set; }
    public void MovementHandle()
    {
        _transform.Translate((MoveSpeed * Time.deltaTime) * MoveDirection);
    }

    public void DirectionHandle()
    {
        if (_transform.position.x < _endPos.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void AnimationHandle()
    {
        
    }

    #endregion

    #region IAttackable Implementation

    public float AttackDamage { get; set; }
    public float AttackCoolDown { get; set; }
    public float AttackTimer { get; set; }
    public float AttackRange { get; set; }
    public IDamageable Target { get; set; }
    public void Attack()
    {
        _currentTarget.TakeDamage(AttackDamage);
    }

    #endregion
}


#if UNITY_EDITOR

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyController enemyController = (EnemyController)target;
        
        EditorGUILayout.LabelField("Stats");
        EditorGUILayout.LabelField("Health", enemyController.Health.ToString());
        EditorGUILayout.LabelField("Move Speed", enemyController.MoveSpeed.ToString());
    }
}
#endif