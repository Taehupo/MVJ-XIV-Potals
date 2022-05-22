using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanAttackController : AttackController
{
    public override void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.time >= nextAtkTime)
            {
                //Debug.Log("Attack !");
                animator.SetTrigger("Attack");
                nextAtkTime = Time.time + 1f / attackRate;
                
                attackHitbox.SetActive(true);
                List<Collider2D> hitTargets = new List<Collider2D>();
                Physics2D.OverlapCollider(attackHitbox.GetComponent<BoxCollider2D>(), contactFilter, hitTargets); 
                foreach (Collider2D hitTarget in hitTargets)
                {
                    //Debug.Log("Attacking " + hitTarget.name + " !");
                    hitTarget.GetComponent<IDamageable>().TakeDamage(damage);
                    hitTarget.GetComponent<IDamageable>().HitRight = (hitTarget.gameObject.transform.position.x > gameObject.transform.position.x);
                }
                attackHitbox.SetActive(false);
            }
        }
    }
}
