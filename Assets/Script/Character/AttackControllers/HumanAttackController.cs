using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttackController : AttackController
{
    public new void Attack()
    {
        attackHitbox.SetActive(true);
    }
}
