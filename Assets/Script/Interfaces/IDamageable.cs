using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    #region Public Manipulators
    public bool HitRight { get; set; }

    public void TakeDamage(int damage);

    #endregion

}
