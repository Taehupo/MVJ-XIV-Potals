using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForthEnemy : Enemy
{

    [SerializeField] private int contactDamage;

    [SerializeField]
    GameObject patrolPointOne;

    [SerializeField]
    GameObject patrolPointTwo;

    [SerializeField]
    float speed;

    bool passedPoint1 = false;
    bool passedPoint2 = true;
    public bool isAttacking = false;
    protected static bool s_IsFlipRight;

    [SerializeField]
    float attackDistance;

    private void Awake()
    {
        CanBeStaggered = true;
        patrolPointOne.transform.SetParent(null, true);
        patrolPointTwo.transform.SetParent(null,true);
    }

	private void FixedUpdate()
	{
        if (CanBeStaggered && IsStaggered)
        {
            Rigidbody2D.velocity = new Vector2(_healthManager.GetHitLocation()*35f, 20f);
            IsStaggered = false;
        }
        else 
        {
            if (Vector2.Distance(transform.position, CharacterManager.Instance.gameObject.transform.position) <= attackDistance && !isAttacking)
            {
                Attack();
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }

            if (Vector2.Distance(gameObject.transform.position, patrolPointOne.transform.position) <= 0.3f && !passedPoint1)
            {
                passedPoint1 = true;
                passedPoint2 = false;
            }
            if (Vector2.Distance(gameObject.transform.position, patrolPointTwo.transform.position) <= 0.3f && !passedPoint2)
            {
                passedPoint1 = false;
                passedPoint2 = true;
            }

            if (!isAttacking)
            {
                if (!passedPoint1 && passedPoint2)
                {
                    Rigidbody2D.velocity = new Vector2(patrolPointOne.transform.position.x - gameObject.transform.position.x, 0).normalized * speed;
                }
                if (passedPoint1 && !passedPoint2)
                {
                    Rigidbody2D.velocity = new Vector2(patrolPointTwo.transform.position.x - gameObject.transform.position.x, 0).normalized * speed;
                }
            }
            else
            {
                Rigidbody2D.velocity = Vector2.zero;
            }
            _spriteManager.SetFloat("Speed", Mathf.Abs(Rigidbody2D.velocity.x));
            Flip(Rigidbody2D.velocity.x < 0);
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<HealthManager>() != null)
        {
            // Debug.Log("Touched " + collision.collider.name);
            collision.collider.GetComponent<HealthManager>().TakeHit(contactDamage, this.gameObject);
        }
        else
        {
            // Debug.Log(collision.collider.name + " was not player");
        }
    }

    protected override void Defeat()
    {
        if (GetComponent<DropSystem>() is not null)
        {
            GetComponent<DropSystem>().CalculateDrops();
        }

        Destroy(gameObject);
    }

    protected override void Attack()
    {
        
        _spriteManager.SetTrigger("Attack");
        attackHitbox.SetActive(true);
        var hitTargets = new List<Collider2D>();
        Physics2D.OverlapCollider(attackHitbox.GetComponent<BoxCollider2D>(), ContactFilter, hitTargets);
        foreach (Collider2D hitTarget in hitTargets)
        {
            Debug.Log("Attacking " + hitTarget.name + " !");
            hitTarget.GetComponent<HealthManager>().TakeHit(hitDamage, this.gameObject);
        }

        isAttacking = false;
        attackHitbox.SetActive(false);
    }
    public void Flip(bool isRight)
    {
        // Change le sens de la hitbox si le sprite a chang?Ede sens
        if (isRight != _spriteManager.Flip(isRight))
        {
            FlipHitbox(isRight);
        }
    }

    public void FlipHitbox(bool isRight)
    {
        Vector2 tmp = attackHitbox.GetComponent<Collider2D>().offset;
        tmp.x *= -1;
        attackHitbox.GetComponent<Collider2D>().offset = tmp;
    }
}
