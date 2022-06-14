using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatMovementController : MovementController
{
	#region Members

	public override ECharacterShape Shape { get => ECharacterShape.Rat; }

	float currentJumpTime = 0f;

	#endregion


	#region Public Manipulators

	public override float Move(InputAction.CallbackContext context)
	{
		Debug.Log("Reading Move : " + context.phase + "\n");
		if (context.phase == InputActionPhase.Started)
		{
			isMoving = true;
			CharacterManager.Instance.Flip(context.ReadValue<Vector2>().x < 0);
		}
		if (context.phase == InputActionPhase.Canceled)
		{
			isMoving = false;
		}
		//Debug.Log(context.ReadValue<Vector2>());
		moveForce = context.ReadValue<Vector2>();
		return moveForce.x;
	}
	public override bool Jump(InputAction.CallbackContext context)
	{
		Debug.Log("Reading jump : " + context.phase + "\n");
		if (context.phase == InputActionPhase.Started)
		{
			isJumping = true;
			isCrouching = false;
		}
		if (context.phase == InputActionPhase.Canceled)
		{
			isJumping = false;
			// Prevents double jump
			currentJumpTime = MaxJumpTime;
		}
		return isJumping;
	}
	public override bool Crouch(InputAction.CallbackContext context)
	{
		return false;
	}

	#endregion


	#region Inherited Manipulators

	protected override void Awake()
	{
		base.Awake();
	}

	void FixedUpdate()
	{
		if (isStaggered)
		{
			CharacterManager.Instance.rb.velocity = new Vector2(10f * CharacterManager.Instance.GetHitLocation(), 10f);
			isStaggered = false;
		}
		else
		{
			if ((isMoving && IsGrounded) || (isMoving && !IsGrounded && !isCollidingInAir))
			{
				CharacterManager.Instance.rb.velocity = new Vector2(Speed * moveForce.x, CharacterManager.Instance.rb.velocity.y);
			}
			else
			{
				CharacterManager.Instance.rb.velocity = new Vector2(0, CharacterManager.Instance.rb.velocity.y);
			}

			if (isJumping)
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
			isCollidingInAir = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !IsGrounded)
		{
			isCollidingInAir = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !IsGrounded)
		{
			isCollidingInAir = false;
		}
	}

	#endregion
}
