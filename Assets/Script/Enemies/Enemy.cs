using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    #region Members

    [SerializeField] private int maxHealth;
    HealthManager healthManager;


    protected bool canBeStaggered;
    private bool isStaggered = false;

    #endregion

    #region Public Manipulators


    #endregion

    #region Inherited Manipulators

    // Start is called before the first frame update
    void Start()
    {
        healthManager = gameObject.AddComponent<HealthManager>();
        healthManager.SetMaxHealth(maxHealth);
        healthManager.onHurt += Hurt;
        healthManager.onDefeat += Defeat;
        healthManager.invincibleTimer.OnEnd = () => { healthManager.StopInvincibility(); gameObject.GetComponent<SpriteRenderer>().enabled = true; };
    }

    private void FixedUpdate()
    {
        if (isStaggered)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(healthManager.GetHitLocation()*10f, 10f);
            isStaggered = false;
        }

        if (healthManager.IsInvincible())
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
        }
    }

    #endregion

    #region Private Manipulators

    private void Defeat()
    {
    }
    private void Hurt()
    {
        isStaggered = true;
    }

    #endregion

}
