using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFlag : PickupItem
{
    [SerializeField] EEventFlag associatedFlag = EEventFlag.count;

    private void Start()
    {
        // Check flag on GM
        if (GameManager.instance.activeEventFlags.Contains(associatedFlag))
            Destroy(gameObject);
    }

    public override void Effect(Collider2D collision)
    {
        GameManager.instance.AddFlag(associatedFlag);
    }

}
