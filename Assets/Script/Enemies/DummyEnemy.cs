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
        if (collision.collider.GetComponent<CharacterManager>() != null)
        {
            Debug.Log("Touched " + collision.collider.name);
            collision.collider.GetComponent<CharacterManager>().TakeDamage(contactDamage);
        }
        else
        {
            Debug.Log(collision.collider.name + " was not player");
        }
    }
}
