using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeItem : PickupItem
{
    [SerializeField] EHealthUpgradeFlag associatedFlag = EHealthUpgradeFlag.count;

    [SerializeField] int upgradeAmount = 2;

    private void Awake()
    {
        // Check flag on GM

    }

    public override void Effect(Collider2D collision)
    {
        // Check if flag already in GM
        HealthManager healthManager = collision.GetComponent<HealthManager>();
        if (healthManager != null)
            healthManager.SetMaxHealth(healthManager.GetMaxHealth() + upgradeAmount);
        // Send flag to GM
    }

}
