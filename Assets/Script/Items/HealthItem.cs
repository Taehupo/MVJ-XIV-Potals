using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : PickupItem
{
    [SerializeField] int amountHealed = 2;

    public override void Effect(Collider2D collision)
    {
        collision.GetComponent<HealthManager>().Heal(amountHealed);
    }
}
