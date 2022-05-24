using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovementController : MovementController
{
	#region Members

	Vector2 moveForce;
	float speed;
	float jumpSpeed;
	[SerializeField] float maxJumpTime = 0.15f;
	float currentJumpTime = 0f;

	bool isCollidingInAir = false;

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
			currentJumpTime = maxJumpTime;
		}
	}

	#endregion


	#region Inherited Manipulators

	protected override void Awake()
	{
		base.Awake();

		// get speed value from CharacterManager (this will change)
		speed = CharacterManager.Instance.CharacterSpeed;
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
			if ((isMoving && CharacterManager.Instance.IsGrounded) || (isMoving && !CharacterManager.Instance.IsGrounded && !isCollidingInAir))
			{
				CharacterManager.Instance.rb.velocity = new Vector2(speed * moveForce.x, CharacterManager.Instance.rb.velocity.y);
			}
			else
			{
				CharacterManager.Instance.rb.velocity = new Vector2(0, CharacterManager.Instance.rb.velocity.y);
			}

			if (isJumping)
			{
				if (CharacterManager.Instance.IsGrounded)
				{
					CharacterManager.Instance.IsGrounded = false;
					currentJumpTime = 0f;
				}

				// Let player jumps higher if held
				if (currentJumpTime < maxJumpTime)
				{
					CharacterManager.Instance.rb.velocity = new Vector2(CharacterManager.Instance.rb.velocity.x, CharacterManager.Instance.CharacterJumpForce);
					currentJumpTime += Time.deltaTime;
				}
			}

			Animator.SetFloat("Speed", Mathf.Abs(CharacterManager.Instance.rb.velocity.x));
		}
	}

	void Update()
	{
		Vector2 boxCastOrigin = gameObject.transform.position;
		boxCastOrigin.y += CharacterManager.Instance.GroundingOffset;
		boxCastOrigin.x += CharacterManager.Instance.BoxCastXOffset;
		RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, new Vector3(0.35f, 0.1f, 1.0f), 0.0f, Vector2.down, 0.1f);
		if (hit.collider != null)
		{
			if (hit.collider.tag == "Platform")
			{
				CharacterManager.Instance.IsGrounded = true;
				Animator.SetBool("Grounded", true);
			}
			else
			{
				CharacterManager.Instance.IsGrounded = false;
				Animator.SetBool("Grounded", false);
			}
		}
		else
		{
			CharacterManager.Instance.IsGrounded = false;
			Animator.SetBool("Grounded", false);
		}
	}

	override public void Draw()
	{
		Vector2 boxCastOrigin = gameObject.transform.position;
		boxCastOrigin.y += CharacterManager.Instance.GroundingOffset;
		boxCastOrigin.x += CharacterManager.Instance.BoxCastXOffset;
		Gizmos.DrawWireCube(boxCastOrigin, new Vector3(0.35f, 0.1f, 1.0f));
	}

	#endregion


	#region Private Manipulators

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !CharacterManager.Instance.IsGrounded)
		{
			isCollidingInAir = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !CharacterManager.Instance.IsGrounded)
		{
			isCollidingInAir = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if ((collision.gameObject.tag == "Platform" || collision.collider.tag == "Wall") && !CharacterManager.Instance.IsGrounded)
		{
			isCollidingInAir = false;
		}
	}

	#endregion
}
