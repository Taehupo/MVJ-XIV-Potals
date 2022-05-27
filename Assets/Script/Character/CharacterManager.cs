using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterManager : Damageable
{
	#region Members

	public GameObject humanAttackHitbox;
	public LayerMask attackLayerMask;

	public Animator animator;
	public SpriteRenderer spriteRenderer;

	public static CharacterManager Instance { get; private set; }

	public ShapeController ShapeController { get; private set; }
	public SpriteManager SpriteManager { get; private set; }

	public Rigidbody2D rb { get; private set; }


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
		// error control
		if (MovementController == null)
		{
			Debug.LogError("CharacterManager.Move() Error : MovementController is null");
			return;
		}

		if (IsAlive())
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

		if (IsAlive())
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

		if (IsAlive())
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
			SpriteManager.ChangeShape((ECharacterShape)nextShape);
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
		invincibleTimer.OnEnd = () => { StopInvincibility(); SpriteManager.StopBlink(); };
	}

    #endregion


    #region Private Manipulators

    void CreateSubComponents()
	{
		ShapeController = gameObject.AddComponent<ShapeController>();
		SpriteManager = gameObject.AddComponent<SpriteManager>();
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

    public override void Hurt()
    {
		animator.SetTrigger("Hurt");
		MovementController.Stagger();
		SpriteManager.Blink();
    }

    public override void Defeat()
    {
        throw new NotImplementedException();
    }
	#endregion
}
