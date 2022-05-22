using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    #region Members

    [SerializeField]private int maxHealth;

    private int currentHealth;

    protected bool canBeStaggered;
    private bool isInvicible = false;
    private Timer invincibleTimer;
    private bool isStaggered = false;

    private bool hitRight = false;

    public bool HitRight { get => hitRight; set => hitRight = value; }

    #endregion

    #region Public Manipulators

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0 && !isInvicible)
        {
            currentHealth -= damage;
            Debug.Log("AÅE");
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().SetTrigger("Hurt");
            }
            isStaggered = true;
            isInvicible = true;
            invincibleTimer.StartTimer(1);
        }

        if (currentHealth <= 0)
            Defeat();
    }

    #endregion

    #region Inherited Manipulators

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        invincibleTimer = gameObject.AddComponent<Timer>();
        invincibleTimer.OnEnd = () => { isInvicible = false; gameObject.GetComponent<SpriteRenderer>().enabled = true; };
    }

    private void FixedUpdate()
    {
        if (isStaggered)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((hitRight?1:-1)*10f, 10f);
            isStaggered = false;
        }

        if (isInvicible)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
        }
    }

    #endregion

    #region Private Manipulators

    private void Defeat()
    {
        Debug.Log("Dead");
        /*Destroy(this);*/
    }

    #endregion

}
