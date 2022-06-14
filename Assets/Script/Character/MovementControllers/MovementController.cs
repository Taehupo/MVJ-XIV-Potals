using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MovementController : MonoBehaviour
{
	#region Members

	public virtual ECharacterShape Shape { get => ECharacterShape.count; }

	/// <summary>IsCrouching</summary>
	public static Action<bool> OnCrouch;

	/// <summary>IsGrounded</summary>
	public static Action<bool> OnGrounded;

	protected Vector2 moveForce;

	public static bool IsGrounded { get; protected set; }
	public static bool IsCrouching { get; private set; }
	protected static bool isMoving;
	protected static bool isJumping;
	protected static bool isStaggered;
	protected static bool isCollidingInAir;

	protected CharacterManager m_CharacterManager;

	CharacterShapeProperties m_ShapeProperties;
	Timer speedModifierTimer;
	List<float> speedModifier;
	private float crouchingSpeedModifier = 0.4f;

	#endregion


	#region Accessors

	protected float Speed { get =>  m_ShapeProperties != null ? m_ShapeProperties.Speed * getSpeedModifier() : 10 * getSpeedModifier(); }
	protected float JumpForce { get => m_ShapeProperties != null ? m_ShapeProperties.JumpForce * getJumpModifier() : 10; }
	protected float MaxJumpTime { get => m_ShapeProperties != null ? m_ShapeProperties.MaxJumpTime * getMaxJumpModifier() : 0.15f; }
	protected float GroundingOffset { get => m_ShapeProperties != null ? m_ShapeProperties.GroundingOffset : 0; }
	protected float BoxCastXOffset { get => m_ShapeProperties != null ? m_ShapeProperties.BoxCastXOffset : 0; }

	#endregion


	#region Public Manipulators

	public abstract float Move(InputAction.CallbackContext context);
	public abstract bool Jump(InputAction.CallbackContext context);
	public abstract bool Crouch(InputAction.CallbackContext context);
	public void SlowDown(float effectTime, float divider)
	{
		speedModifierTimer.StartTimer(effectTime);
		speedModifier.Add(divider);
	}

	public abstract void Draw();

	public static void Stagger() { isStaggered = true; }

    #endregion


    #region Inherited Manipulators

    protected virtual void Awake()
	{
		// get a parent reference
		m_CharacterManager = CharacterManager.Instance;
		speedModifierTimer = gameObject.AddComponent<Timer>();
		speedModifierTimer.OnEnd = () => { speedModifier.RemoveAt(speedModifier.Count-1); };

		speedModifier = new List<float>();
	}

    private void Start()
    {
		// get Shape Properties
		m_ShapeProperties = m_CharacterManager.ShapeController.GetShapeProperties(Shape);
	}

	#endregion


	#region Protected Manipulators

	protected float getSpeedModifier()
	{
		float speedM = 1f;
		for (int i = 0; i < speedModifier.Count; i++)
			speedM *= speedModifier[i];
		return speedM * (isCrouching ? crouchingSpeedModifier : 1f);
	}

	protected float getJumpModifier()
	{
		if (GameManager.instance != null && GameManager.instance.activeEventFlags.Contains(EEventFlag.HighJumpUnlocked))
			return 1.2f;
		return 1f;
	}

	protected float getMaxJumpModifier()
	{
		if (GameManager.instance != null && GameManager.instance.activeEventFlags.Contains(EEventFlag.HighJumpUnlocked))
			return 2f;
		return 1f;
	}

	protected void SetCrouching(bool isCrouching)
    {
		IsCrouching = isCrouching;
		OnCrouch?.Invoke(IsCrouching);
	}

	#endregion
}
