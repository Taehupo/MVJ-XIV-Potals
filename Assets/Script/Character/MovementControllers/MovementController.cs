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

	protected static Vector2 s_MoveForce;
	protected static Vector2 s_LockedMoveForce;

	public static bool IsMovementLock { get; private set; }
	public static bool IsGrounded { get; protected set; }
	public static bool IsCrouching { get; private set; }
	protected static bool s_IsMoving;
	protected static bool s_IsJumping;
	protected static bool s_IsStaggered;
	protected static bool s_IsCollidingInAir;

	protected static bool s_LockedIsMoving;
	protected static bool s_IsFlipRight;

	protected CharacterManager m_CharacterManager;

	CharacterShapeProperties m_ShapeProperties;
	Timer m_SpeedModifierTimer;
	List<float> m_SpeedModifier;
	private float m_CrouchingSpeedModifier = 0.0f;
	protected float fallMultiplier = 1f;

	#endregion


	#region Accessors

	protected float Speed { get =>  m_ShapeProperties != null ? m_ShapeProperties.Speed * GetSpeedModifier() : 10 * GetSpeedModifier(); }
	protected float JumpForce { get => m_ShapeProperties != null ? m_ShapeProperties.JumpForce * GetJumpModifier() : 10; }
	protected float MaxJumpTime { get => m_ShapeProperties != null ? m_ShapeProperties.MaxJumpTime * GetMaxJumpModifier() : 0.15f; }
	protected float GroundingOffset { get => m_ShapeProperties != null ? m_ShapeProperties.GroundingOffset : 0; }
	protected float BoxCastXOffset { get => m_ShapeProperties != null ? m_ShapeProperties.BoxCastXOffset : 0; }

	#endregion


	#region Public Manipulators

	public abstract float Move(InputAction.CallbackContext context);
	public abstract bool Jump(InputAction.CallbackContext context);
	public abstract bool Crouch(InputAction.CallbackContext context);
	public void SlowDown(float effectTime, float divider)
	{
		if (m_SpeedModifierTimer.enabled)
			m_SpeedModifierTimer.StopTimer();
		m_SpeedModifierTimer.StartTimer(effectTime);
		m_SpeedModifier.Add(divider);
	}

	public abstract void Draw();

	public static void Stagger() { s_IsStaggered = true; }

	public void LockMovement()
    {
		IsMovementLock = true;

		// disable crouch
		SetCrouching(false);
	}

	public void BeforeUnlockMovement()
    {
		s_MoveForce = new();
		CharacterManager.Instance.Flip(s_IsFlipRight);
		s_IsMoving = false;
	}

	public void UnlockMovement()
    {
		IsMovementLock = false;
		s_MoveForce = s_LockedMoveForce;
		CharacterManager.Instance.Flip(s_IsFlipRight);
		s_IsMoving = s_LockedIsMoving;
	}

    #endregion


    #region Inherited Manipulators

    protected virtual void Awake()
	{
		// get a parent reference
		m_CharacterManager = CharacterManager.Instance;
		m_SpeedModifierTimer = gameObject.AddComponent<Timer>();
		m_SpeedModifierTimer.OnEnd = () => { m_SpeedModifier.RemoveAt(m_SpeedModifier.Count-1); };

		m_SpeedModifier = new List<float>();
	}

    private void Start()
    {
		// get Shape Properties
		m_ShapeProperties = m_CharacterManager.ShapeController.GetShapeProperties(Shape);
	}

	#endregion


	#region Protected Manipulators

	protected float GetSpeedModifier()
	{
		float speedM = 1f;
		for (int i = 0; i < m_SpeedModifier.Count; i++)
			speedM *= m_SpeedModifier[i];
		return speedM * (IsCrouching ? m_CrouchingSpeedModifier : 1f);
	}

	protected float GetJumpModifier()
	{
		if (GameManager.instance != null && GameManager.instance.activeEventFlags.Contains(EEventFlag.HighJumpUnlocked))
			return 1.2f;
		return 1f;
	}

	protected float GetMaxJumpModifier()
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
