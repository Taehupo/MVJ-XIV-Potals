using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeItem : PickupItem
{
    override public void SoundEffect()
    {
        CharacterManager.Instance.PlayUpgrade();
    }
}
