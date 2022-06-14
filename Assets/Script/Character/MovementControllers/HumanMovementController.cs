using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovementController : MovementController
{
    #region Members

    public override ECharacterShape Shape { get => ECharacterShape.Human; }

	float currentJumpTime = 0f;

	#endregion


	#region Public Manipulators

	public override float Move(InputAction.CallbackContext context)
	{
		Debug.Log("Reading Move : " + context.phase + "\n");
		if (context.phase == InputActionPhase.Started)
		{
			s_LockedIsMoving = true;
			s_IsFlipRight = context.ReadValue<Vector2>().x < 0;
			if (!IsMovementLock)
            {
				CharacterManager.Instance.Flip(s_IsFlipRight);
				s_IsMoving = true;
			}
		}

		if (context.phase == InputActionPhase.Canceled)
		{
			s_LockedIsMoving = false;
			if (!IsMovementLock)
			{
				s_IsMoving = false;
			}
		}

		s_LockedMoveForce = context.ReadValue<Vector2>();
		if (!IsMovementLock)
			s_MoveForce = s_LockedMoveForce;

		return s_MoveForce.x;
	}

	public override bool Jump(InputAction.CallbackContext context)
	{
		Debug.Log("Reading jump : " + context.phase + "\n");
		if (context.phase == InputActionPhase.Started)
		{
			s_IsJumping = true;
			SetCrouching(false);
		}
		if (context.phase == InputActionPhase.Canceled)
		{
			s_IsJumping = false;
			// Prevents double jump
			currentJumpTime = MaxJumpTime;
		}
		return s_IsJumping;
	}
    public override bool Crouch(InputAction.CallbackContext context)
	{
		// MovementLock disable crouch
		if (IsMovementLock)
		{
			SetCrouching(false);
			return IsCrouching;
		}

		if (IsGrounded)
		{
			if (context.phase == InputActionPhase.Started)
			{
				SetCrouching(true);
			}
			if (context.phase == InputActionPhase.Canceled)
			{
				SetCrouching(false);
			}
		}
		else
			SetCrouching(false);

		return IsCrouching;
    }

    #endregion


    #region Inherited Manipulators

    protected override void Awake()
	{
		base.Awake();
	}

	void FixedUpdate()
	{
		if (s_IsStaggered)
		{
			CharacterManager.Instance.rb.velocity = new Vector2(10f*CharacterManager.Instance.GetHitLocation(), 10f);
			s_IsStaggered = false;
		}
		else
        {
			if ((s_IsMoving && IsGrounded) || (s_IsMoving && !IsGrounded && !s_IsCollidingInAir))
			{
				CharacterManager.Instance.rb.velocity = new Vector2(Speed * s_MoveForce.x, CharacterManager.Instance.rb.velocity.y);
			}
			else
			{
				CharacterManager.Instance.rb.velocity = new Vector2(0, CharacterManager.Instance.rb.velocity.y);
			}

			if (s_IsJumping && !IsMovementLock)
			{
				if (IsGrounded)
				{
					IsGrounded = false;
					currentJumpTime = 0f;
				}

				// Let player jumps higher if held
				if (currentJumpTime < MaxJumpTime)
				{
					CharacterManager.Instance.rb.velocity = new Vector2(CharacterManager.Instance.rb.velocity.x, JumpForce);
					currentJumpTime += Time.deltaTime;
				}
			}
		}
	}

	private void Update()
	{
		Vector2 boxCastOrigin = gameObject.transform.position;
		boxCastOrigin.y += GroundingOffset;
		boxCastOrigin.x += BoxCastXOffset;
		RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCastOrigin, new Vector3(0.35f, 0.1f, 1.0f), 0.0f, Vector2.down, 0.1f);
		bool tmpIsGrounded = false;
		foreach (RaycastHit2D hit in hits)
		{
			if (hit.collider is null) 
				continue;
			if (hit.collider.CompareTag("Platform"))
			{
				tmpIsGrounded = true;
			}
		}
		IsGrounded = tmpIsGrounded;
		OnGrounded?.Invoke(IsGrounded);
	}

	public override void Draw()
	{
		Vector2 boxCastOrigin = gameObject.transform.position;
		boxCastOrigin.y += GroundingOffset;
		boxCastOrigin.x += BoxCastXOffset;
		Gizmos.DrawWireCube(boxCastOrigin, new Vector3(0.35f, 0.1f, 1.0f));
	}

	#endregion


	#region Private Manipulators

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !IsGrounded)
		{
			s_IsCollidingInAir = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !IsGrounded)
		{
			s_IsCollidingInAir = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !IsGrounded)
		{
			s_IsCollidingInAir = false;
		}
	}

	#endregion
}
