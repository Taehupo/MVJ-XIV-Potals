using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MovementController : MonoBehaviour
{
	#region Members

	protected static bool isMoving;
	protected static bool isJumping;

	protected CharacterManager m_CharacterManager;

	#endregion


	#region Public Manipulators

	public abstract void Move(InputAction.CallbackContext context);

	public abstract void Jump(InputAction.CallbackContext context);

	public abstract void Draw();

    #endregion


    #region Inherited Manipulators

    protected virtual void Awake()
	{
		// get a parent reference
		m_CharacterManager = CharacterManager.Instance;
	}

	#endregion


	#region Protected Manipulators

	#endregion
}
