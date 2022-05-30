using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelinItem : PickupItem
{
    [SerializeField] int ammo = 2;

    public override void Effect(Collider2D collision)
    {
        CharacterManager.Instance.AddAmmo(ammo);
    }
}
