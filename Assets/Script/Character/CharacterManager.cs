using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour, IDamageable
{
	#region Members

	public GameObject humanAttackHitbox;
	public LayerMask attackLayerMask;

	public Animator animator;
	public SpriteRenderer spriteRenderer;

	public static CharacterManager Instance { get; private set; }

	public ShapeController ShapeController { get; private set; }


	public Rigidbody2D rb { get; private set; }

	public int MaxHealth { get => maxHealth; set => maxHealth = value; }
	public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

	[SerializeField]
	int maxHealth;

	[SerializeField]
	int currentHealth;


	public List<CharacterShapeProperties> ShapesProperties;

	#endregion


	#region Flags
	public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
	public bool SideCollision { get => sideCollision; set => sideCollision = value; }

	private bool hitRight = false;
	public bool HitRight { get => hitRight; set => hitRight = value; }

	[SerializeField]
	bool isGrounded = false;
	bool sideCollision = false;
	bool isAlive = true;
	bool isInvicible = false;
	Timer invincibleTimer;
	#endregion


	#region Accessors

	public MovementController MovementController { get => ShapeController.MovementController; }
	public AttackController AttackController { get => ShapeController.AttackController; }

	#endregion


	#region PlayerInput

	public void Move(InputAction.CallbackContext context)
	{
		// error control
		if (MovementController == null)
		{
			Debug.LogError("CharacterManager.Move() Error : MovementController is null");
			return;
		}

		if (isAlive)
		{
			MovementController.Move(context);
		}
	}

	public void Jump(InputAction.CallbackContext context)
	{
		// error control
		if (MovementController == null)
		{
			Debug.LogError("CharacterManager.Jump() Error : MovementController is null");
			return;
		}

		if (isAlive)
		{
			animator.SetTrigger("Jump");
			MovementController.Jump(context);
		}
	}

	public void Crouch(InputAction.CallbackContext context)
	{
		//TODO : Add a crouch
	}

	public void Attack(InputAction.CallbackContext context)
	{
		// error control
		if (AttackController == null)
		{
			Debug.LogError("CharacterManager.Attack() Error : AttackController is null");
			return;
		}

		if (isAlive)
		{
			AttackController.Attack(context);
		}
	}

	public void ChangeShape(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			int nextShape = (int)(ShapeController.CharacterShape + 1) % (int)(ECharacterShape.count);
			Debug.Log(nextShape);
			ShapeController.ChangeShape((ECharacterShape)nextShape);
		}
	}

	#endregion


	#region Public Manipulators

	public void Flip(bool isRight)
    {
		// Flip la hitbox d'attaque
		if (isRight != spriteRenderer.flipX)
		{
			Vector2 tmp = humanAttackHitbox.GetComponent<Collider2D>().offset;
			tmp.x *= -1;
			humanAttackHitbox.GetComponent<Collider2D>().offset = tmp;
		}
		spriteRenderer.flipX = isRight;
    }

	public void TakeDamage(int damage)
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
	}

    private void Start()
    {
		CurrentHealth = MaxHealth;
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
	}

	// Preview cast Area on Player seleted if Gizmo is activated
	void OnDrawGizmosSelected()
    {
		if (ShapeController != null)
		{
			AttackController.Draw();
		}
		if (ShapeController != null)
		{
			MovementController.Draw();
		}
    }

	#endregion
}
