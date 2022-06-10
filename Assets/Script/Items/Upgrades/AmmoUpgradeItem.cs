using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUpgradeItem : PickupItem
{
    [SerializeField] EAmmoUpgradeFlag associatedFlag = EAmmoUpgradeFlag.count;

    private int upgradeAmount = 2;

    private void Start()
    {
        // Check flag on GM
        if (GameManager.instance.aquiredAmmoUpgrades.Contains(associatedFlag))
            Destroy(gameObject);
    }

    public override void Effect(Collider2D collision)
    {
        CharacterManager characterManager = CharacterManager.Instance;
        if (characterManager != null)
            characterManager.SetMaxAmmo(characterManager.MaxAmmo + upgradeAmount);
        GameManager.instance.AddFlag(associatedFlag);
    }

}
