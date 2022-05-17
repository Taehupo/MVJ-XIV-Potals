using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackController
{
	#region Members

	#endregion


	#region Public Manipulators

	public void Attack();
    public void Draw();

    #endregion


    #region Inherited Manipulators

    public Transform AttackPoint { get; set; }
    public float AttackRange { get; set; }
    public LayerMask EnemyLayer { get; set; }
    public int Damage { get; set; }  

    #endregion


    #region Private Manipulators

    #endregion
}
