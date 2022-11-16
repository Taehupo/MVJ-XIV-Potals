using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : MonoBehaviour
{
	#region Members

	[SerializeField] private Collider2D characterCollider;
	[SerializeField] private Collider2D crouchingCharacterCollider;
	private AudioSource audioSource;

	public GameObject humanAttackHitbox;
	public LayerMask attackLayerMask;
	public AudioClip sfx_footsteps, sfx_attack, sfx_shoot, sfx_landing, sfx_hurt, sfx_transform, sfx_transformRat;

	[SerializeField] private int maxJavelinAmmo = 5;
	public int currentJavelinAmmo = 5;

	public Action onJavelinAmmoChange;

	public static CharacterManager Instance { get; private set; }

	public ShapeController ShapeController { get; private set; }
	public SpriteManager SpriteManager { get; private set; }
	public HealthManager HealthManager { get; private set; }

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
	public int MaxAmmo { get => maxJavelinAmmo; }
	#endregion


	#region PlayerInput

	public void Move(InputAction.CallbackContext context)
	{
		if (CanAct())
		{
			MovementController.Move(context);
		}
	}

	public void Jump(InputAction.CallbackContext context)
	{
		if (CanAct())
		{
			if (context.started)
				SpriteManager.SetTrigger("Jump");
			MovementController.Jump(context);
		}
	}

	public void Crouch(InputAction.CallbackContext context)
	{
		if (CanAct())
        {
			MovementController.Crouch(context);
        }
	}

	public void Attack(InputAction.CallbackContext context)
	{
		if (CanAct())
		{
			if (AttackController.Attack(context))
			{
				MovementController.SlowDown(1f/AttackController.GetAttackRate(), 0.5f);
				SpriteManager.SetTrigger("Attack");
				PlayAttack();
			}
		}
	}

	public void SubAttack(InputAction.CallbackContext context)
	{
		if (CanAct())
		{
			if (AttackController.SubAttack(context))
			{
				MovementController.SlowDown(1f / AttackController.GetAttackRate(), 0.5f);
				SpriteManager.SetTrigger("Attack");
				PlayShoot();
			}
		}
	}

	public void ChangeShape(InputAction.CallbackContext context)
	{
		if (CanAct())
		{
			if (context.started && !MovementController.IsMovementLock)
			{
				List<ECharacterShape> shapes = GameManager.instance.usableShapes;
				int currentShape = shapes.IndexOf(ShapeController.CharacterShape);
				int nextShape = (int)(currentShape + 1) % (int)(shapes.Count);
				// Debug.Log(nextShape);
				
				// Teste la hauteur pour permettre la transformation 
				// (si plafond trop bas en rat, empêche de retransformer)
				CapsuleCollider2D newHitbox = ShapeController.GetShapeProperties((ECharacterShape)nextShape).Hitbox.GetComponent<CapsuleCollider2D>();
				Vector3 origin = new Vector3(0f, 1f, 0f) + GetComponent<Transform>().position;
				
				RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, newHitbox.size, 0.0f, Vector2.up, 0.1f);
				bool canTransform = true;
				foreach (RaycastHit2D hit in hits)
				{
					if (hit.collider is null)
						continue;
					if (hit.collider.CompareTag("Platform"))
					{
						canTransform = false;
					}
				}

				if (canTransform)
				{
					SetShape((ECharacterShape)nextShape);
					PlayTransform((ECharacterShape)nextShape);
				}
			}
		}
	}

	#endregion


	#region Public Manipulators

	public bool CanAct()
	{
		return HealthManager.IsAlive() && !PauseMenu.GameIsPaused;
	}
	public void Flip(bool isRight)
	{
		// Change le sens de la hitbox si le sprite a chang�Ede sens
		if (isRight != SpriteManager.Flip(isRight))
		{
			ShapeController.AttackController.FlipHitbox(isRight);
		}
	}

	public int GetHitLocation() { return HealthManager.GetHitLocation(); }

	public void AddAmmo(int amount)
	{
		SetAmmo(Math.Min(maxJavelinAmmo, currentJavelinAmmo + amount));
	}
	public void RemoveAmmo(int amount)
	{
		SetAmmo(Math.Max(currentJavelinAmmo - amount, 0));
	}
	public void SetAmmo(int amount)
	{
		currentJavelinAmmo = amount;
		onJavelinAmmoChange?.Invoke();
	}
	public void SetMaxAmmo(int amount)
	{
		maxJavelinAmmo = amount;
		currentJavelinAmmo = maxJavelinAmmo;
		onJavelinAmmoChange?.Invoke();
	}
	public void SetShape(ECharacterShape shape)
	{
		Collider2D newHitbox = ShapeController.GetShapeProperties(shape).Hitbox;

		ShapeController.ChangeShape(shape);
		SpriteManager.ChangeShape(shape);
		characterCollider.offset = newHitbox.offset;
		characterCollider.GetComponent<CapsuleCollider2D>().size = newHitbox.GetComponent<CapsuleCollider2D>().size;
	
		/*crouchingCharacterCollider.offset = ShapeController.GetShapeProperties(shape).CrouchHitbox.offset;
		crouchingCharacterCollider.GetComponent<CapsuleCollider2D>().size = 
			ShapeController.GetShapeProperties(shape).CrouchHitbox.GetComponent<CapsuleCollider2D>().size;*/
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
		audioSource = GetComponent<AudioSource>();

		CreateSubComponents();

		// initialise colliders to unCrouch
		OnCrouch(false);

		HealthManager.SetMaxHealth(maxHealth);
		HealthManager.invincibleTimer.OnEnd = () => { HealthManager.StopInvincibility(); SpriteManager.StopBlink(); };

		// register call back
		HealthManager.onHurt += OnHurt;
		HealthManager.onDefeat += OnDefeat;
		MovementController.OnCrouch += OnCrouch;
		MovementController.OnGrounded += OnGrounded;
	}

    private void OnDestroy()
	{
		// unregister call back
		MovementController.OnCrouch -= OnCrouch;
		MovementController.OnGrounded -= OnGrounded;
	}

    private void FixedUpdate()
	{
		if (MovementController.IsGrounded)
			SpriteManager.SetFloat("Speed", Mathf.Abs(this.rb.velocity.x));
	}

    #endregion


    #region Private Manipulators

    private void CreateSubComponents()
	{
		ShapeController = gameObject.AddComponent<ShapeController>();
		SpriteManager = gameObject.AddComponent<SpriteManager>();
		HealthManager = gameObject.AddComponent<HealthManager>();
	}

	private void OnCrouch(bool isCrouching)
	{
		SpriteManager.SetBool("Crouch", isCrouching);
		if (isCrouching)
		{
			characterCollider.enabled = false;
			crouchingCharacterCollider.enabled = true;
		}
		else
		{
			characterCollider.enabled = true;
			crouchingCharacterCollider.enabled = false;
		}
	}

	private void OnGrounded(bool isGrounded)
	{
		if (isGrounded && !SpriteManager.GetBool("Grounded")) 
		{
			PlayLanding();
		}
		SpriteManager.SetBool("Grounded", isGrounded);
	}

	private void OnHurt()
	{
		SpriteManager.SetTrigger("Hurt");
		MovementController.Stagger();
		SpriteManager.Blink();
		PlayHurt();
	}

	private void OnDefeat()
	{
		//throw new NotImplementedException();
		if (GameManager.instance.GetComponent<SaveSystem>() != null)
		{
			GameManager.instance.LoadGame();
			Debug.Log("Read file");
		}
	}

	public void PlayFootstep()
	{
		if (!audioSource.isPlaying)
		{
			audioSource.PlayOneShot(sfx_footsteps, 0.7f);
		}		
	}
	public void PlayLanding()
	{
		audioSource.PlayOneShot(sfx_landing, 0.5f);
	}
	public void PlayAttack()
	{
		audioSource.PlayOneShot(sfx_attack, 0.6f);
	}
	public void PlayHurt()
	{
		audioSource.PlayOneShot(sfx_hurt, 0.7f);
	}
	public void PlayShoot()
	{
		audioSource.PlayOneShot(sfx_shoot, 0.4f);
	}
	public void PlayTransform(ECharacterShape transform)
	{
		switch(transform)
		{
			case ECharacterShape.Rat:
				audioSource.PlayOneShot(sfx_transformRat, 0.5f);
				break;
			default:
				audioSource.PlayOneShot(sfx_transform, 0.5f);
				break;
		}
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
