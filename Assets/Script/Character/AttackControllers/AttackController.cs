using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AttackController : MonoBehaviour
{
    #region Members

    public virtual ECharacterShape Shape { get => ECharacterShape.count; }

    public GameObject attackHitbox;
    public LayerMask enemyLayer;

    protected float nextAtkTime = 0f;
    protected ContactFilter2D contactFilter = new();

    protected Animator Animator { get; set; }

    protected CharacterManager m_CharacterManager;

    CharacterShapeProperties m_ShapeProperties;

    #endregion


    #region Accessors

    protected float AttackDamage { get => m_ShapeProperties != null ? m_ShapeProperties.AttackDamage : 1; }
    protected float AttackRate { get => m_ShapeProperties != null ? m_ShapeProperties.AttackRate : 3; }
    protected GameObject SubAttackPrefab { get => m_ShapeProperties.SubAttackPrefab; }
    protected GameObject SubAttackPosition { get => m_ShapeProperties.SubAttackPosition; }

    #endregion


    #region Public Manipulators

    public abstract bool Attack(InputAction.CallbackContext context);

    public abstract bool SubAttack(InputAction.CallbackContext context);
    public void Draw() { }
    public void FlipHitbox(bool isRight)
    {
        Vector2 tmp = attackHitbox.GetComponent<Collider2D>().offset;
        tmp.x *= -1;
        attackHitbox.GetComponent<Collider2D>().offset = tmp;
    }
    public float getAttackRate() { return AttackRate; }

    public GameObject getSubAttackPrefab() { return SubAttackPrefab; }
    public GameObject getSubAttackPosition() { return SubAttackPosition; }

    #endregion


    #region Inherited Manipulators

    protected virtual void Awake()
    {
        // get a parent reference
        m_CharacterManager = CharacterManager.Instance;
        enemyLayer = m_CharacterManager.attackLayerMask;
        contactFilter.SetLayerMask(enemyLayer);
        
        // TODO use Properties HitBox 
        attackHitbox = m_CharacterManager.humanAttackHitbox;
        attackHitbox.SetActive(false);
    }

    private void Start()
    {
        // get Shape Properties
        m_ShapeProperties = m_CharacterManager.ShapeController.GetShapeProperties(Shape);
    }

    #endregion


    #region Private Manipulators

    #endregion
}
