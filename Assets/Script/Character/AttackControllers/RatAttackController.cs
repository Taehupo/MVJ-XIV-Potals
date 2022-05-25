using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatAttackController : AttackController
{
    #region Members

    public override ECharacterShape Shape { get => ECharacterShape.Rat; }

    #endregion

    public override void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Rat attacks!! not effective...");
    }
}
