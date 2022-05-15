using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
	#region Members

	bool isMoving;

	float speed;

	Vector2 moveForce;

	#endregion


	#region Public Manipulators

	public void Move(InputAction.CallbackContext context)
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

    #endregion


    #region Inherited Manipulators

    private void Awake()
    {
		// register callback
		ShapeController.OnShapeChanged += OnShapeChanged;
	}

	// Start is called before the first frame update
	void Start()
	{
		speed = CharacterManager.Instance.CharacterSpeed;
	}

	private void OnDestroy()
	{
		// unregister callback
		ShapeController.OnShapeChanged -= OnShapeChanged;
	}


	// Update is called once per frame
	void Update()
	{

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
	}

    #endregion


    #region Private Manipulators

    void OnShapeChanged(ECharacterShape shape)
    {

    }

	#endregion
}
