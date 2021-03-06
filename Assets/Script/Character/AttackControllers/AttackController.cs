using System;
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
    protected GameObject SubAttackPrefab2 { get => m_ShapeProperties.SubAttackPrefab2; }
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
    public float GetAttackRate() { return AttackRate; }

    public GameObject GetSubAttackPrefab() { return SubAttackPrefab; }
    public GameObject GetSubAttackPrefab2() { return SubAttackPrefab2; }
    public GameObject GetSubAttackPosition() { return SubAttackPosition; }

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

    private void FixedUpdate()
    {
        if (Time.time < nextAtkTime)
        {
            attackHitbox.SetActive(true);
            var hitTargets = new List<Collider2D>();
            Physics2D.OverlapCollider(attackHitbox.GetComponent<BoxCollider2D>(), contactFilter, hitTargets); 
            foreach (Collider2D hitTarget in hitTargets)
            {
                //Debug.Log("Attacking " + hitTarget.name + " !");
                HealthManager healthManager = hitTarget.GetComponent<HealthManager>();
                if (healthManager != null)
                    healthManager.TakeHit((int)AttackDamage, this.gameObject);
                PilumDoor door = hitTarget.GetComponent<PilumDoor>();
                if (door != null)
                    door.Hit(0);
            }
            attackHitbox.SetActive(false);
        }
    }

    #endregion


    #region Private Manipulators

    #endregion
}
