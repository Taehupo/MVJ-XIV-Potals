using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : Enemy
{
    [SerializeField] private int contactDamage;

    private void Awake()
    {
        canBeStaggered = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Damageable>() != null)
        {
            Debug.Log("Touched " + collision.collider.name);
            collision.collider.GetComponent<Damageable>().TakeHit(contactDamage, this.gameObject);
        }
        else
        {
            Debug.Log(collision.collider.name + " was not player");
        }
    }
}
