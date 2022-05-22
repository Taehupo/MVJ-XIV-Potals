using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatAttackController : AttackController
{
    public override void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Rat attacks!! not effective...");
    }
}
