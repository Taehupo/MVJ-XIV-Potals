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

	public static CharacterManager Instance { get; private set; }

	public ShapeController ShapeController { get; private set; }
	public SpriteManager SpriteManager { get; private set; }
	private HealthManager healthManager;

	public Rigidbody2D rb { get; private set; }
	[SerializeField] private int maxHealth = 1;

	public List<CharacterShapeProperties> ShapesProperties;
	public List<CharacterShapeVisuals> ShapesVisuals;

	#endregion


	#region Flags
	public bool SideCollision { get => sideCollision; set => sideCollision = value; }

	bool sideCollision = false;
	#endregion


	#region Accessors

	public MovementController MovementController { get => ShapeController.MovementController; }
	public AttackController AttackController { get => ShapeController.AttackController; }

	#endregion


	#region PlayerInput

	public void Move(InputAction.CallbackContext context)
	{
		if (healthManager.IsAlive())
		{
			MovementController.Move(context);
		}
	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (healthManager.IsAlive())
		{
			MovementController.Jump(context);
			SpriteManager.SetTrigger("Jump");
		}
	}

	public void Crouch(InputAction.CallbackContext context)
	{
		//TODO : Add a crouch
	}

	public void Attack(InputAction.CallbackContext context)
	{
		if (healthManager.IsAlive())
		{
			if (AttackController.Attack(context))
				SpriteManager.SetTrigger("Attack");
		}
	}

	public void ChangeShape(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			int nextShape = (int)(ShapeController.CharacterShape + 1) % (int)(ECharacterShape.count);
			Debug.Log(nextShape);
			ShapeController.ChangeShape((ECharacterShape)nextShape);
			SpriteManager.ChangeShape((ECharacterShape)nextShape);
		}
	}

	#endregion


	#region Public Manipulators
	public void Flip(bool isRight)
	{
		// Change le sens de la hitbox si le sprite a chang� de sens
		if (isRight != SpriteManager.Flip(isRight))
		{
			ShapeController.AttackController.FlipHitbox(isRight);
		}
	}

	public int GetHitLocation() { return healthManager.GetHitLocation(); }

	public void Hurt()
	{
		SpriteManager.SetTrigger("Hurt");
		MovementController.Stagger();
		SpriteManager.Blink();
	}

	public void Defeat()
	{
		throw new NotImplementedException();
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

		healthManager.SetMaxHealth(maxHealth);
		healthManager.invincibleTimer.OnEnd = () => { healthManager.StopInvincibility(); SpriteManager.StopBlink(); };
		healthManager.onHurt += Hurt;
		healthManager.onDefeat += Defeat;
	}

    private void FixedUpdate()
    {
        SpriteManager.SetBool("Grounded", MovementController.IsGrounded());
		if (MovementController.IsGrounded())
			SpriteManager.SetFloat("Speed", Mathf.Abs(this.rb.velocity.x));
	}

    #endregion


    #region Private Manipulators

    void CreateSubComponents()
	{
		ShapeController = gameObject.AddComponent<ShapeController>();
		SpriteManager = gameObject.AddComponent<SpriteManager>();
		healthManager = gameObject.AddComponent<HealthManager>();
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
