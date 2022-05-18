using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
	#region Members
	public Transform humanAttackPoint;
	public float humanAttackRange;
	public LayerMask attackLayerMask;
	public int humanAttackDamage;

	public Animator animator;

	public static CharacterManager Instance { get; private set; }

	public ShapeController ShapeController { get; private set; }
	public MovementController CurrentMovementController { get; private set; }
	public IAttackController CurrentAttackController { get; private set; }


	public Rigidbody2D rb { get; private set; }

	public float CharacterSpeed { get => characterSpeed; set => characterSpeed = value; }
	public float CharacterJumpForce { get => characterJumpForce; set => characterJumpForce = value; }
	public float GroundingOffset { get => groundingOffset; set => groundingOffset = value; }
	public float BoxCastXOffset { get => boxCastXOffset; set => boxCastXOffset = value; }

	[SerializeField]
	float characterSpeed;

	[SerializeField]
	float characterJumpForce;

	private Dictionary<ECharacterShape, MovementController> ShapeToMovementController = new();
	private Dictionary<ECharacterShape, IAttackController> ShapeToAttackController = new();

	[SerializeField]
	float groundingOffset = -0.4f;

	[SerializeField]
	float boxCastXOffset;

	#endregion

	#region Flags
	public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
	public bool SideCollision { get => sideCollision; set => sideCollision = value; }

	[SerializeField]
	bool isGrounded = false;
	bool sideCollision = false;
	#endregion


	#region Public Manipulators
	public void Move(InputAction.CallbackContext context)
	{
		if(CurrentMovementController == null)
        {
			Debug.LogError("CharacterManager.Move() Error : CurrentMovementController is null");
			return;
        }

		CurrentMovementController.Move(context);
	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (CurrentMovementController == null)
		{
			Debug.LogError("CharacterManager.Jump() Error : CurrentMovementController is null");
			return;
		}

		CurrentMovementController.Jump(context);
	}

	public void Crouch(InputAction.CallbackContext context)
	{
		//TODO : Add a crouch
	}

	public void Attack(InputAction.CallbackContext context)
	{
		if (context.started)
        {
			animator.SetTrigger("Attack");
			CurrentAttackController.Attack();
		}
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
		IAttackController humanAttackController = new HumanAttackController(humanAttackPoint, humanAttackRange, attackLayerMask, humanAttackDamage);
		ShapeToAttackController.Add(ECharacterShape.Human, humanAttackController);
		
		CurrentAttackController = humanAttackController;

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

	void OnDrawGizmosSelected()
    {
		if (CurrentAttackController != null)
		{
			CurrentAttackController.Draw();
		}
		if (CurrentMovementController != null)
		{
			CurrentMovementController.Draw();
		}

    }

	#endregion
}
