using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MovementController : MonoBehaviour
{
	#region Members

	public virtual ECharacterShape Shape { get => ECharacterShape.count; }

	protected Vector2 moveForce;

	protected static bool isGrounded;
	protected static bool isMoving;
	protected static bool isJumping;
	protected static bool isStaggered;
	protected static bool isCollidingInAir;

	protected CharacterManager m_CharacterManager;

	CharacterShapeProperties m_ShapeProperties;

	#endregion


	#region Accessors

	protected float Speed { get =>  m_ShapeProperties != null ? m_ShapeProperties.Speed : 10; }
	protected float JumpForce { get => m_ShapeProperties != null ? m_ShapeProperties.JumpForce : 10; }
	protected float MaxJumpTime { get => m_ShapeProperties != null ? m_ShapeProperties.MaxJumpTime : 0.15f; }
	protected float GroundingOffset { get => m_ShapeProperties != null ? m_ShapeProperties.GroundingOffset : 0; }
	protected float BoxCastXOffset { get => m_ShapeProperties != null ? m_ShapeProperties.BoxCastXOffset : 0; }

	#endregion


	#region Public Manipulators

	public abstract float Move(InputAction.CallbackContext context);

	public abstract bool Jump(InputAction.CallbackContext context);

	public abstract void Draw();

	public static void Stagger() { isStaggered = true; }
	public bool IsGrounded() { return isGrounded; }

    #endregion


    #region Inherited Manipulators

    protected virtual void Awake()
	{
		// get a parent reference
		m_CharacterManager = CharacterManager.Instance;
	}

    private void Start()
    {
		// get Shape Properties
		m_ShapeProperties = m_CharacterManager.ShapeController.GetShapeProperties(Shape);
	}

    #endregion


    #region Protected Manipulators

    #endregion
}
