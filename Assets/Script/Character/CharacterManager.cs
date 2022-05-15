using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    #region Members

    public static CharacterManager Instance { get; private set; }
	public float CharacterSpeed { get => characterSpeed; set => characterSpeed = value; }

	MovementController movementController;
	AttackController AttackController;

	[SerializeField]
	float characterSpeed;

    public Rigidbody2D rb;

	//private ShapeController ShapeController { get; private set; }

	#endregion


	#region Public Manipulators
	public void Move(InputAction.CallbackContext context)
	{
		movementController.Move(context);
	}

	public void Jump(InputAction.CallbackContext context)
	{
		//TODO : Add A JUMP
	}

	public void Crouch(InputAction.CallbackContext context)
	{
		//TODO : Add a crouch
	}

	public void Attack(InputAction.CallbackContext context)
	{
		//TODO : Add an attack
	}
	#endregion


	#region Inherited Manipulators

	void Awake()
    {
        // do not keep multiple instances
        if (Instance != null)
        {
            Debug.LogError("CharacterManager.Awake() Error - Multiple CharacterManager : " + gameObject.name + " " + Instance.gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        movementController = gameObject.AddComponent<MovementController>();
        AttackController = gameObject.AddComponent<AttackController>();

        CreateSubComponents();
    }

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	#endregion


	#region Private Manipulators

	void CreateSubComponents()
    {

    }

    #endregion
}
