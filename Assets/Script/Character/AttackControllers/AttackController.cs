using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AttackController : MonoBehaviour
{
    #region Members

    public GameObject attackHitbox;
    public LayerMask enemyLayer;
    public int damage;
    public int attackRate;
    public Animator animator;

    protected float nextAtkTime = 0f;
    protected ContactFilter2D contactFilter;

    #endregion


    #region Public Manipulators
    public AttackController()
    {
        ContactFilter2D contactFilter = new();
        contactFilter.SetLayerMask(enemyLayer);
    }

    public abstract void Attack(InputAction.CallbackContext context);
    public void Draw() { }
    public void Set(GameObject _attackHitbox, LayerMask _enemyLayer, int _damage, int _attackRate, Animator _animator)
    {
        this.attackHitbox = _attackHitbox;
        this.enemyLayer = _enemyLayer;
        this.damage = _damage;
        this.attackRate = _attackRate;
        this.animator = _animator;
    }

    #endregion


    #region Inherited Manipulators

    #endregion


    #region Private Manipulators

    #endregion
}
