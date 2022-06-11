using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyEnemy : Enemy
{
    [SerializeField] private int contactDamage;

    private void Awake()
    {
        CanBeStaggered = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<HealthManager>() != null)
        {
            Debug.Log("Touched " + collision.collider.name);
            collision.collider.GetComponent<HealthManager>().TakeHit(contactDamage, this.gameObject);
        }
        else
        {
            Debug.Log(collision.collider.name + " was not player");
        }
    }

    protected override void Defeat()
    {
        if (GetComponent<DropSystem>() is not null)
        {
            GetComponent<DropSystem>().CalculateDrops();
        }

        Destroy(gameObject);
    }

    protected override void Attack()
    {
        _spriteManager.SetTrigger("Attack");
        attackHitbox.SetActive(true);
        var hitTargets = new List<Collider2D>();
        Physics2D.OverlapCollider(attackHitbox.GetComponent<BoxCollider2D>(), ContactFilter, hitTargets); 
        foreach (Collider2D hitTarget in hitTargets)
        {
            //Debug.Log("Attacking " + hitTarget.name + " !");
            hitTarget.GetComponent<HealthManager>().TakeHit(hitDamage, this.gameObject);
        }
        
        attackHitbox.SetActive(false);
    }
}
