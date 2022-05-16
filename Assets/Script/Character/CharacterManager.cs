using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    #region Members

    public static CharacterManager Instance { get; private set; }

	public MovementController MovementController { get; private set; }
	public AttackController AttackController { get; private set; }
	public ShapeController ShapeController { get; private set; }

	public float CharacterSpeed { get => characterSpeed; set => characterSpeed = value; }


	[SerializeField]
	float characterSpeed;

    public Rigidbody2D rb;

	#endregion


	#region Public Manipulators
	public void Move(InputAction.CallbackContext context)
	{
		MovementController.Move(context);
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

		rb = GetComponent<Rigidbody2D>();

		CreateSubComponents();

		// register callback
		ShapeController.OnShapeChanged += OnShapeChanged;
	}

    private void OnDestroy()
    {
		// unregister callback
		ShapeController.OnShapeChanged -= OnShapeChanged;
	}

    void Start()
	{
	}

	#endregion


	#region Private Manipulators

	void CreateSubComponents()
	{
		MovementController = gameObject.AddComponent<MovementController>();
		AttackController = gameObject.AddComponent<AttackController>();
		ShapeController = gameObject.AddComponent<ShapeController>();
	}

	void OnShapeChanged(ECharacterShape shape)
    {

    }

	#endregion
}
