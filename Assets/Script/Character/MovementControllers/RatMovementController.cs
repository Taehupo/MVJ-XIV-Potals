using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatMovementController : MovementController
{
	#region Members

	Vector2 moveForce;

	bool isCollidingInAir = false;
	public override ECharacterShape Shape { get => ECharacterShape.Rat; }

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
		if ((isMoving && CharacterManager.Instance.IsGrounded) || (isMoving && !CharacterManager.Instance.IsGrounded && !isCollidingInAir))
		{
			CharacterManager.Instance.rb.velocity = new Vector2(Speed * moveForce.x, CharacterManager.Instance.rb.velocity.y);
		}
		else
		{
			CharacterManager.Instance.rb.velocity = new Vector2(0, CharacterManager.Instance.rb.velocity.y);
		}

		if (isJumping && CharacterManager.Instance.IsGrounded)
		{
			Debug.Log("Rat jumps at force : " + JumpForce);
			CharacterManager.Instance.rb.velocity = new Vector2(CharacterManager.Instance.rb.velocity.x, JumpForce);
			CharacterManager.Instance.IsGrounded = false;
		}
		Animator.SetFloat("Speed", Mathf.Abs(CharacterManager.Instance.rb.velocity.x));
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
		boxCastOrigin.y += GroundingOffset;
		boxCastOrigin.x += BoxCastXOffset;
		Gizmos.DrawWireCube(boxCastOrigin, new Vector3(0.35f, 0.1f, 1.0f));
	}

	#endregion


	#region Private Manipulators

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Platform" && !CharacterManager.Instance.IsGrounded)
		{
			//Debug.Log("I am touching platform !");
			isCollidingInAir = true;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Platform" && !CharacterManager.Instance.IsGrounded)
		{
			//Debug.Log("I am touching platform !");
			isCollidingInAir = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Platform" && !CharacterManager.Instance.IsGrounded)
		{
			//Debug.Log("I am touching NOT platform anymore !");
			isCollidingInAir = false;
		}
	}

	#endregion
}
