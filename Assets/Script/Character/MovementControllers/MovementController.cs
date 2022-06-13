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
	protected static bool isCrouching = false;

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
	public float getSpeedModifier()
    {
		float speedM = 1f;
		for (int i = 0; i < speedModifier.Count; i++)
			speedM *= speedModifier[i];
		return speedM * (isCrouching?crouchingSpeedModifier:1f);
    }
	public float getJumpModifier()
	{
		if (GameManager.instance.activeEventFlags.Contains(EEventFlag.HighJumpUnlocked))
			return 1.2f;
		return 1f;
	}
	public float getMaxJumpModifier()
	{
		if (GameManager.instance.activeEventFlags.Contains(EEventFlag.HighJumpUnlocked))
			return 2f;
		return 1f;
	}

	public abstract void Draw();

	public static void Stagger() { isStaggered = true; }
	public bool IsGrounded() { return isGrounded; }

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

    #endregion
}
