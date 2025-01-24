using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AttackRangeRenderer : MonoBehaviour
{
    [SerializeField] private int _segments = 50;
    [SerializeField] private float _radius;
    
    [Header("Components")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _transform;

    private LineRenderer _lineRenderer;

    void Start()
    {
        //Retrieve Values
        _radius = DataManager.Instance.PlayerData.radiusToPick;
        
        //Retrieve Components
        _playerTransform = GamePlayManager.Instance.Player.transform;
        _transform = GetComponent<Transform>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _segments + 1;
        _lineRenderer.useWorldSpace = false;
        
        
        CreatePoints();
    }

    void CreatePoints()
    {
        float angle = 0f;
        for (int i = 0; i < _segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;
            _lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / _segments;
        }
    }
}