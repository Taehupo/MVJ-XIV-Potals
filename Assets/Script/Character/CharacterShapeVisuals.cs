using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Properties that define a CharacterShape used by the CharacterControllers
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/CharacterShapeVisuals", order = 1)]
public class CharacterShapeVisuals : ScriptableObject
{
    #region Members
    
    [SerializeField]
    private ECharacterShape m_Shape = ECharacterShape.count;

    [SerializeField]
    private Sprite m_ShapeSprite;

    [SerializeField]
    private RuntimeAnimatorController m_AnimatorController;

    [SerializeField]
    private Sprite m_UI_Icon;

    #endregion

    #region Accessors

    public ECharacterShape Shape { get => m_Shape; }

    public Sprite ShapeSprite { get => m_ShapeSprite; }

    public RuntimeAnimatorController AnimatorController { get => m_AnimatorController; }
    
    public Sprite UIIcon { get => m_UI_Icon; }

    #endregion
}
