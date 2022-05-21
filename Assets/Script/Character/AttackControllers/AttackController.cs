using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    #region Members

    public GameObject attackHitbox;
    public LayerMask enemyLayer;
    public int damage;
    public int attackRate;

    #endregion


    #region Public Manipulators

    public void Attack() { }
    public void Draw() { }
    public void Set(GameObject _attackHitbox, LayerMask _enemyLayer, int _damage, int _attackRate)
    {
        this.attackHitbox = _attackHitbox;
        this.enemyLayer = _enemyLayer;
        this.damage = _damage;
        this.attackRate = _attackRate;
    }

    #endregion


    #region Inherited Manipulators

    #endregion


    #region Private Manipulators

    #endregion
}
