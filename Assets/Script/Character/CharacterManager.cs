using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
	#region Members

	public GameObject humanAttackHitbox;
	public LayerMask attackLayerMask;
	public int humanAttackDamage;
	public int humanAttackRate;

	public Animator animator;
	public SpriteRenderer spriteRenderer;

	public static CharacterManager Instance { get; private set; }

	public ShapeController ShapeController { get; private set; }
	public MovementController CurrentMovementController { get; private set; }
	public AttackController CurrentAttackController { get; private set; }


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
	private Dictionary<ECharacterShape, AttackController> ShapeToAttackController = new();

	[SerializeField]
	float groundingOffset;

	[SerializeField]
	float boxCastXOffset;

	public int MaxHealth { get => maxHealth; set => maxHealth = value; }
	public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

	[SerializeField]
	int maxHealth;

	[SerializeField]
	int currentHealth;

	#endregion

	#region Flags
	public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
	public bool SideCollision { get => sideCollision; set => sideCollision = value; }

	[SerializeField]
	bool isGrounded = false;
	bool sideCollision = false;
	bool isAlive = true;
	bool isInvicible = false;
	Timer invincibleTimer;
	#endregion


	#region Public Manipulators
	public void Move(InputAction.CallbackContext context)
	{
		if(CurrentMovementController == null)
        {
			Debug.LogError("CharacterManager.Move() Error : CurrentMovementController is null");
			return;
        }

		if (isAlive)
		{
			CurrentMovementController.Move(context);
		}		
	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (CurrentMovementController == null)
		{
			Debug.LogError("CharacterManager.Jump() Error : CurrentMovementController is null");
			return;
		}

		if (isAlive)
		{
			animator.SetTrigger("Jump");
			CurrentMovementController.Jump(context);
		}		
	}

	public void Crouch(InputAction.CallbackContext context)
	{
		//TODO : Add a crouch
	}

	public void Attack(InputAction.CallbackContext context)
	{
		if (isAlive)
		{
			CurrentAttackController.Attack(context);
		}
	}

	public void Flip(bool isRight)
    {
		spriteRenderer.flipX = isRight;
    }

	public void TakeDamage(int damage) // Doit déterminer le côté de collision et attente du putain de gestionnaire de temps de Super Lulu, je sais que tu me lis putain mets le
	{
		if (isAlive && !isInvicible)
		{
			CurrentHealth -= damage;
			Debug.Log("Took " + damage + ", health remaining : " + CurrentHealth);
			animator.SetTrigger("Hurt");
			MovementController.Stagger();
			isInvicible = true;
			invincibleTimer.StartTimer(1);
		}		

		if (CurrentHealth <= 0)
        {
			Debug.Log("Player is ded =(");
			isAlive = false;
		}
	}

	public void Heal(int amount)
	{
		CurrentHealth += amount;

		if (CurrentHealth > MaxHealth)
		{
			CurrentHealth = MaxHealth;
		}
		Debug.Log("Healed " + amount + ", health : " + CurrentHealth);
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

		humanAttackHitbox.SetActive(false);

		invincibleTimer = gameObject.AddComponent<Timer>();
		invincibleTimer.OnEnd = () => { isInvicible = false; spriteRenderer.enabled = true; };

		// register callback
		ShapeController.OnShapeChanged += OnShapeChanged;
	}

    private void Start()
    {
		CurrentHealth = MaxHealth;
    }

    private void OnDestroy()
    {
		// unregister callback
		ShapeController.OnShapeChanged -= OnShapeChanged;

		// clear dictionnary
		ShapeToMovementController.Clear();
		ShapeToAttackController.Clear();
	}

    private void FixedUpdate()
    {
        if (isInvicible)
        {
			spriteRenderer.enabled = !spriteRenderer.enabled;
		}
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
		AttackController humanAttackController = gameObject.AddComponent<HumanAttackController>(); //= new HumanAttackController(humanAttackPoint, humanAttackRange, attackLayerMask, humanAttackDamage);
		humanAttackController.Set(humanAttackHitbox,attackLayerMask,humanAttackDamage,humanAttackRate, animator);
		ShapeToAttackController.Add(ECharacterShape.Human, humanAttackController);
		// add other shape related AttackController

		CurrentAttackController = humanAttackController;

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

	// Preview cast Area on Player seleted if Gizmo is activated
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
