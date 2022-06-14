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

    [SerializeField]
    private Collider2D m_hitBox;
    [SerializeField]
    private Collider2D m_crouchHitBox;

    // AttackProperties
    [SerializeField]
    private GameObject m_AttackHitbox;
    [SerializeField]
    private float m_AttackDamage = 1;
    [SerializeField]
    private float m_AttackRate = 3;


    // SubAttackProperties
    [SerializeField]
    private GameObject m_SubAttackPrefab;
    [SerializeField]
    private GameObject m_SubAttackPrefab2;
    [SerializeField]
    private GameObject m_SubAttackPosition;
    [SerializeField]
    private float m_SubAttackDamage = 1;
    [SerializeField]
    private float m_SubAttackRate = 3;

    #endregion


    #region Accessors

    public ECharacterShape Shape { get => m_Shape; }

    // MovementProperties
    public float Speed { get => m_Speed; }
    public float JumpForce { get => m_JumpForce; }
    public float MaxJumpTime { get => m_MaxJumpTime; }
    public float GroundingOffset { get => m_GroundingOffset; }
    public float BoxCastXOffset { get => m_BoxCastXOffset; }
    public Collider2D Hitbox { get => m_hitBox; }
    public Collider2D CrouchHitbox { get => m_crouchHitBox; }


    // AttackProperties
    public GameObject AttackHitbox { get => m_AttackHitbox; }
    public float AttackDamage { get => m_AttackDamage; }
    public float AttackRate { get => m_AttackRate; }

    // SubAttackProperties
    public GameObject SubAttackPrefab { get => m_SubAttackPrefab; }
    public GameObject SubAttackPrefab2 { get => m_SubAttackPrefab2; }
    public GameObject SubAttackPosition { get => m_SubAttackPosition; }
    public float SubAttackDamage { get => m_SubAttackDamage; }
    public float SubAttackRate { get => m_SubAttackRate; }

    #endregion
}
