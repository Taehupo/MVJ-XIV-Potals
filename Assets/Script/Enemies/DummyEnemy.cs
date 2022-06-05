using System.Collections;
using System.Collections.Generic;
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
        if (GetComponent<DropSystem>() != null)
        {
            GetComponent<DropSystem>().CalculateDrops();
        }

        Destroy(gameObject);
    }
}
