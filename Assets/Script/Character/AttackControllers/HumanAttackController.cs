using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttackController : IAttackController
{

    public HumanAttackController(Transform _attackPoint, float _attackRange, LayerMask _enemyLayer, int _damage)
    {
        this._attackPoint = _attackPoint;
        this._attackRange = _attackRange;
        this._enemyLayer = _enemyLayer;
        this._damage = _damage;
    }
    private Transform _attackPoint;
    public Transform AttackPoint { get => _attackPoint; set => _attackPoint = value; }
    private float _attackRange;
    public float AttackRange { get => _attackRange; set => _attackRange = value; }
    private LayerMask _enemyLayer;
    public LayerMask EnemyLayer { get => _enemyLayer; set => _enemyLayer = value; }
    private int _damage;
    public int Damage { get => _damage; set => _damage = value; }

    public void Attack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(AttackPoint.position, new Vector2(AttackRange,AttackRange/2), 0.0f, EnemyLayer);
        foreach(Collider2D hitTarget in hitTargets)
        {
            hitTarget.GetComponent<IDamageable>().TakeDamage(Damage);
        }
    }

    public void Draw()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireCube(AttackPoint.position, new Vector3(AttackRange, AttackRange/2));
    }
}
