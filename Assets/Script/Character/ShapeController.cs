using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    #region Members

    public ECharacterShape CharacterShape { get; private set; }

    public MovementController MovementController { get; private set; }
    public AttackController AttackController { get; private set; }

    /// <summary>
    /// Previous CharacterShape , Next CharacterShape
    /// </summary>
    public static Action<ECharacterShape, ECharacterShape> OnShapeChanged; // is this used?
    public Action OnShapeChange;

    private Dictionary<ECharacterShape, MovementController> m_ShapeToMovementController = new();
    private Dictionary<ECharacterShape, AttackController> m_ShapeToAttackController = new();
    private Dictionary<ECharacterShape, CharacterShapeProperties> m_ShapeToProperties = new();

    private Timer m_BeforeOnMovementUnlocked;
    private Timer m_OnMovementUnlocked;

    #endregion


    #region Accessors

    public CharacterShapeProperties GetShapeProperties(ECharacterShape shape)
    {
        if (!m_ShapeToProperties.ContainsKey(shape))
        {
            Debug.LogError("ShapeController.GetShapeProperties() Error : No CharacterShapeProperties for shape " + shape);
            return null;
        }

        return m_ShapeToProperties[shape];
    }


    #endregion


    #region Public Manipulators

    public void ChangeShape(ECharacterShape shape)
    {
        SetShape(shape);
    }

    public void LockMovement()
    {
        MovementController.LockMovement();
    }

    /// <summary>
    /// Unlock Movement in 2 time 
    /// first stop the character after stopDelay then resume movement based on player input after resumeDelay
    /// </summary>
    /// <param name="stopDelay"></param>
    /// <param name="resumeDelay"></param>
    public void UnlockMovement(float stopDelay = 0.2f, float resumeDelay = 0.5f)
    {
        m_BeforeOnMovementUnlocked.OnEnd = () =>
        {
            MovementController.BeforeUnlockMovement();
            m_OnMovementUnlocked.StartTimer(resumeDelay);
        };
        m_BeforeOnMovementUnlocked.StartTimer(stopDelay);
    }

    #endregion


    #region Inherited Manipulators

    private void Awake()
    {
        CreateShapeToProperties();
        CreateSubComponents();

        // set default shape as Human
        SetShape(ECharacterShape.Human);
    }


    private void OnDestroy()
    {
        // clear dictionnary
        m_ShapeToMovementController.Clear();
        m_ShapeToAttackController.Clear();
        m_ShapeToProperties.Clear();
    }

    #endregion


    #region Private Manipulators

    void CreateShapeToProperties()
    {
        foreach (CharacterShapeProperties shapeProperties in CharacterManager.Instance.ShapesProperties)
        {
            if (m_ShapeToProperties.ContainsKey(shapeProperties.Shape))
            {
                Debug.LogError("ShapeController.CreateShapeToProperties() Error : Duplicate CharacterShapeProperties in CharacterManager " + shapeProperties.Shape);
                continue;
            }

            m_ShapeToProperties.Add(shapeProperties.Shape, shapeProperties);
        }
    }

    void CreateSubComponents()
    {
        CreateMovementControllers();
        CreateAttackControllers();


        m_BeforeOnMovementUnlocked = gameObject.AddComponent<Timer>();

        m_OnMovementUnlocked = gameObject.AddComponent<Timer>();
        m_OnMovementUnlocked.OnEnd = () => { MovementController.UnlockMovement(); };
    }

    void CreateMovementControllers()
    {
        MovementController movementController = gameObject.AddComponent<HumanMovementController>();
        movementController.enabled = false;
        m_ShapeToMovementController.Add(ECharacterShape.Human, movementController);

        movementController = gameObject.AddComponent<RatMovementController>();
        movementController.enabled = false;
        m_ShapeToMovementController.Add(ECharacterShape.Rat, movementController);

        // Add other shape related MovementController
    }

    void CreateAttackControllers()
    {
        CharacterShapeProperties shapeProperties = m_ShapeToProperties[ECharacterShape.Human];


        AttackController attackController = gameObject.AddComponent<HumanAttackController>();
        attackController.enabled = false;

        m_ShapeToAttackController.Add(ECharacterShape.Human, attackController);

        attackController = gameObject.AddComponent<RatAttackController>();
        attackController.enabled = false;

        m_ShapeToAttackController.Add(ECharacterShape.Rat, attackController);

        // Add other shape related AttackController
    }

    private void SetShape(ECharacterShape shape)
    {
        if (shape == ECharacterShape.count)
        {
            Debug.LogError("ShapeController.SetShape() Error : Cannot set shape to " + shape);
            return;
        }
        SwapControllers(shape);
        OnShapeChanged?.Invoke(CharacterShape, shape);

        CharacterShape = shape;
        OnShapeChange?.Invoke();
    }

    void SwapControllers(ECharacterShape shape)
    {
        // MovementController
        if (MovementController != null)
            MovementController.enabled = false;

        if (m_ShapeToMovementController[shape] == null)
            Debug.LogError("ShapeController.SwapControllers() Error : No MovementController found for " + shape);
        else
        {
            MovementController = m_ShapeToMovementController[shape];
            MovementController.enabled = true;
        }

        // AttackController
        if (AttackController != null)
            AttackController.enabled = false;

        if (m_ShapeToAttackController[shape] == null)
            Debug.LogError("ShapeController.SwapControllers() Error : No AttackController found for " + shape);
        else
        {
            AttackController = m_ShapeToAttackController[shape];
            AttackController.enabled = true;
        }

        // Swap Other Controller based on shape here
    }

    #endregion
}
