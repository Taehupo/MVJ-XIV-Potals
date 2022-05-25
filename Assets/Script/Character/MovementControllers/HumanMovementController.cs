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

	public override void Move(InputAction.CallbackContext context)
	{
		Debug.Log("Reading Move : " + context.phase + "\n");
		if (context.phase == InputActionPhase.Started)
		{
			isMoving = true;
			CharacterManager.Instance.Flip(context.ReadValue<Vector2>().x > 0);
		}
		if (context.phase == InputActionPhase.Canceled)
		{
			isMoving = false;
		}
		//Debug.Log(context.ReadValue<Vector2>());
		moveForce = context.ReadValue<Vector2>();
	}

	public override void Jump(InputAction.CallbackContext context)
	{
		Debug.Log("Reading jump : " + context.phase + "\n");
		if (context.phase == InputActionPhase.Started)
		{
			isJumping = true;
		}
		if (context.phase == InputActionPhase.Canceled)
		{
			isJumping = false;
			// Prevents double jump
			currentJumpTime = MaxJumpTime;
		}
	}

	#endregion


	#region Inherited Manipulators

	protected override void Awake()
	{
		base.Awake();

		Animator = CharacterManager.Instance.animator;
	}

	void FixedUpdate()
	{
		if (isStaggered)
		{
			CharacterManager.Instance.rb.velocity = new Vector2(10f*(CharacterManager.Instance.HitRight?1:-1), 10f);
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

			Animator.SetFloat("Speed", Mathf.Abs(CharacterManager.Instance.rb.velocity.x));
		}
	}

	void Update()
	{
		Vector2 boxCastOrigin = gameObject.transform.position;
		boxCastOrigin.y += GroundingOffset;
		boxCastOrigin.x += BoxCastXOffset;
		RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, new Vector3(0.35f, 0.1f, 1.0f), 0.0f, Vector2.down, 0.1f);
		if (hit.collider != null)
		{
			if (hit.collider.tag == "Platform")
			{
				IsGrounded = true;
				Animator.SetBool("Grounded", true);
			}
			else
			{
				IsGrounded = false;
				Animator.SetBool("Grounded", false);
			}
		}
		else
		{
			IsGrounded = false;
			Animator.SetBool("Grounded", false);
		}
	}

	override public void Draw()
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
