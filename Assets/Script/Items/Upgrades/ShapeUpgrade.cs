using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeUpgrade : UpgradeItem
{
    [SerializeField] ECharacterShape associatedFlag = ECharacterShape.count;

    private void Start()
    {
        // Check flag on GM
        if (GameManager.instance.usableShapes.Contains(associatedFlag))
            Destroy(gameObject);
    }

    public override void Effect(Collider2D collision)
    {
        GameManager.instance.AddShape(associatedFlag);
        CharacterManager.Instance.SetShape(associatedFlag);
    }

}
