using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProjectileController : MonoBehaviour, IMoveable, IAttackable
{
    public Transform TargetTransform;
    [SerializeField] private Transform _transform;

    
    #region MonoBehaviour Methods

    private void Start()
    {
        MoveSpeed = 9f;
        AttackRange = 0.1f;
        
        Target = TargetTransform.GetComponent<IDamageable>();
        _transform = transform;
    }

    private void Update()
    {
        if (TargetTransform != null)
        {
            MoveDirection = (TargetTransform.position - transform.position).normalized;

            if (Vector2.SqrMagnitude(_transform.position - TargetTransform.position) < AttackRange)
            {
                Target.TakeDamage(Damage);
                Destroy(gameObject);
                return;
            }
            MovementHandle();
            DirectionHandle();
        } 
        else
        {
            Destroy(gameObject);
        }
        
    }

    #endregion

    #region IMoveable Implementation

    public float MoveSpeed { get; set; }
    public Vector2 MoveDirection { get; set; }
    public void MovementHandle()
    {
        transform.Translate(MoveDirection * (Time.deltaTime * MoveSpeed), Space.World);
    }

    public void DirectionHandle()
    {
        Vector3 direction = TargetTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void AnimationHandle()
    {
        throw new NotImplementedException();
    }

    #endregion
    
    #region IAttackable Implementation
    public float Damage { get; set; }
    public float AttackInterval { get; set; }
    public float AttackTimer { get; set; }
    public float AttackRange { get; set; }
    [SerializeField] private IDamageable _target;
    public IDamageable Target
    {
        get
        {
            return _target;
        }
        set => _target = value;
    }

    public void Attack()
    {
        throw new NotImplementedException();
    }
    #endregion
}

//inspector code
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ProjectileController))]
public class ProjectileControllerrEditor : UnityEditor.Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProjectileController self = (ProjectileController)target;
        EditorGUILayout.LabelField("Stats");
        EditorGUILayout.LabelField("Damage", self.Damage.ToString());
        EditorGUILayout.LabelField("Move Speed", self.MoveSpeed.ToString());
        
    }
   
}
#endif