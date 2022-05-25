using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Properties that define a CharacterShape used by the CharacterControllers
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/ShapeProperties", order = 1)]
public class CharacterShapeProperties : ScriptableObject
{
    #region Members

    [SerializeField]
    private ECharacterShape m_Shape = ECharacterShape.count;

    // MovementProperties
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_JumpForce;
    [SerializeField]
    private float m_GroundingOffset;
    [SerializeField]
    private float m_BoxCastXOffset;

    // AttackProperties
    [SerializeField]
    private GameObject m_AttackHitbox;
    [SerializeField]
    private float m_AttackDamage;
    [SerializeField]
    private float m_AttackRate;

    #endregion


    #region Accessors

    public ECharacterShape Shape { get => m_Shape; }

    // MovementProperties
    public float Speed { get => m_Speed; }
    public float JumpForce { get => m_JumpForce; }
    public float GroundingOffset { get => m_GroundingOffset; }
    public float BoxCastXOffset { get => m_BoxCastXOffset; }


    // AttackProperties
    public GameObject AttackHitbox { get => m_AttackHitbox; }
    public float AttackDamage { get => m_AttackDamage; }
    public float AttackRate { get => m_AttackRate; }

    #endregion
}
