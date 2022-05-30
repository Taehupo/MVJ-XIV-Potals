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
                
                attackHitbox.SetActive(true);
                List<Collider2D> hitTargets = new List<Collider2D>();
                Physics2D.OverlapCollider(attackHitbox.GetComponent<BoxCollider2D>(), contactFilter, hitTargets); 
                foreach (Collider2D hitTarget in hitTargets)
                {
                    //Debug.Log("Attacking " + hitTarget.name + " !");
                    hitTarget.GetComponent<HealthManager>().TakeHit((int)AttackDamage, this.gameObject);
                }
                attackHitbox.SetActive(false);
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
                    Vector3 flipPos = SubAttackPosition.transform.position;
                    flipPos.x *= isFlipped ? 1 : -1;

                    Instantiate(SubAttackPrefab, transform.position + flipPos, new Quaternion());
                    CharacterManager.Instance.currentJavelinAmmo--;
                }
            }
        }
        return isAttacking;
    }
}
