using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    float AttackDamage { get; set; }
    float AttackCoolDown { get; set; }
    float AttackTimer { get; set; }
    float AttackRange { get; set; }
    // IDamageable _target;

    IDamageable Target { get; set; }
    void Attack();
}
