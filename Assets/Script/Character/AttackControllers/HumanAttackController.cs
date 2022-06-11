using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanAttackController : AttackController
{
    #region Members

    public override ECharacterShape Shape { get => ECharacterShape.Human; }

    #endregion

    public override bool Attack(InputAction.CallbackContext context)
    {
        bool isAttacking = false;
        if (context.started)
        {
            if (Time.time >= nextAtkTime)
            {
                isAttacking = true;
                //Debug.Log("Attack !");
                nextAtkTime = Time.time + 1f / AttackRate;
            }
        }
        return isAttacking;
    }

    public override bool SubAttack(InputAction.CallbackContext context)
    {
        bool isAttacking = false;
        if (context.started)
        {
            if (Time.time >= nextAtkTime)
            {
                if (CharacterManager.Instance.currentJavelinAmmo > 0)
                {
                    isAttacking = true;
                    //Debug.Log("Attack !");
                    nextAtkTime = Time.time + 1f / AttackRate;

                    // Determine throw position
                    bool isFlipped = CharacterManager.Instance.SpriteManager.IsFlipped();
                    var flipPos = SubAttackPosition.transform.position;
                    flipPos.x *= isFlipped ? 1 : -1;

                    Instantiate(SubAttackPrefab, transform.position + flipPos, new Quaternion());
                    CharacterManager.Instance.RemoveAmmo(1);
                }
            }
        }
        return isAttacking;
    }
}
