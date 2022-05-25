using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Properties that define a CharacterShape used by the CharacterControllers
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/CharacterShapeProperties ", order = 1)]
public class CharacterShapeProperties : ScriptableObject
{
    #region Members

    [SerializeField]
    private ECharacterShape m_Shape = ECharacterShape.count;

    // MovementProperties
    [SerializeField]
    private float m_Speed = 10;
    [SerializeField]
    private float m_JumpForce = 10;
    [SerializeField]
    private float m_MaxJumpTime = 0.15f;
    [SerializeField]
    private float m_GroundingOffset = -0.1f;
    [SerializeField]
    private float m_BoxCastXOffset = -0.03f;

    // AttackProperties
    [SerializeField]
    private GameObject m_AttackHitbox;
    [SerializeField]
    private float m_AttackDamage = 1;
    [SerializeField]
    private float m_AttackRate = 3;

    #endregion


    #region Accessors

    public ECharacterShape Shape { get => m_Shape; }

    // MovementProperties
    public float Speed { get => m_Speed; }
    public float JumpForce { get => m_JumpForce; }
    public float MaxJumpTime { get => m_MaxJumpTime; }
    public float GroundingOffset { get => m_GroundingOffset; }
    public float BoxCastXOffset { get => m_BoxCastXOffset; }


    // AttackProperties
    public GameObject AttackHitbox { get => m_AttackHitbox; }
    public float AttackDamage { get => m_AttackDamage; }
    public float AttackRate { get => m_AttackRate; }

    #endregion
}
