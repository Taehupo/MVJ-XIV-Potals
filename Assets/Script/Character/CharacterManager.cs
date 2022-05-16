using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
    #region Members

    public static CharacterManager Instance { get; private set; }

	public ShapeController ShapeController { get; private set; }
	public MovementController CurrentMovementController { get; private set; }
	public IAttackController CurrentAttackController { get; private set; }

	public float CharacterSpeed { get => characterSpeed; set => characterSpeed = value; }


	[SerializeField]
	float characterSpeed;

    public Rigidbody2D rb;
	private Dictionary<ECharacterShape, MovementController> ShapeToMovementController = new();
	private Dictionary<ECharacterShape, IAttackController> ShapeToAttackController = new();

	#endregion


	#region Public Manipulators
	public void Move(InputAction.CallbackContext context)
	{
		CurrentMovementController.Move(context);
	}

	public void Jump(InputAction.CallbackContext context)
	{
		CurrentMovementController.Jump(context);
	}

	public void Crouch(InputAction.CallbackContext context)
	{
		//TODO : Add a crouch
	}

	public void Attack(InputAction.CallbackContext context)
	{
		CurrentAttackController.Attack();
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

		// clear dictionnary
		ShapeToMovementController.Clear();
		ShapeToAttackController.Clear();
	}

	#endregion


	#region Private Manipulators

	void CreateSubComponents()
	{
		ShapeController = gameObject.AddComponent<ShapeController>();

		CreateMovementControllers();
		CreateAttackControllers();
	}

	void CreateMovementControllers()
    {
		MovementController movementController = gameObject.AddComponent<HumanMovementController>();
		ShapeToMovementController.Add(ECharacterShape.Human, movementController);
		CurrentMovementController = movementController;

		// add other shape related MovementController

	}

	void CreateAttackControllers()
	{
		IAttackController attackController = gameObject.AddComponent<HumanAttackController>();
		ShapeToAttackController.Add(ECharacterShape.Human, attackController);
		CurrentAttackController = attackController;

		// add other shape related AttackController
	}

	void OnShapeChanged(ECharacterShape shape)
	{
		if (shape == ECharacterShape.count)
			return;

		if(ShapeToMovementController[shape] != null)
			CurrentMovementController = ShapeToMovementController[shape];

		if(ShapeToAttackController[shape] != null)
			CurrentAttackController = ShapeToAttackController[shape];

		// Swap Other Controller based on shape here
	}

	#endregion
}
