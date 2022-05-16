using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovementController : MovementController
{
	#region Members

	Vector2 moveForce;
	float speed;

	#endregion


	#region Public Manipulators

	public override void Move(InputAction.CallbackContext context)
	{
		Debug.Log("Reading Move : " + context.phase + "\n");
		if (context.phase == InputActionPhase.Started)
		{
			isMoving = true;
		}
		if (context.phase == InputActionPhase.Canceled)
		{
			isMoving = false;
		}
		Debug.Log(context.ReadValue<Vector2>());
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

		// get speed value from CharacterManager (this will change)
		speed = m_CharacterManager.CharacterSpeed;
	}

	void FixedUpdate()
	{
		if (isMoving)
		{
			CharacterManager.Instance.rb.velocity = new Vector2(speed * moveForce.x, CharacterManager.Instance.rb.velocity.y);
		}
		else
		{
			CharacterManager.Instance.rb.velocity = new Vector2(0, CharacterManager.Instance.rb.velocity.y);
		}

		if (isJumping)
		{
			CharacterManager.Instance.rb.velocity = new Vector2(CharacterManager.Instance.rb.velocity.x, 5);
		}
	}

	#endregion


	#region Private Manipulators

	#endregion
}