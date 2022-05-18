using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    #region Members

    [SerializeField]private int maxHealth;

    private int currentHealth;

    #endregion

    #region Public Manipulators

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("AÅE");

        if (currentHealth <= 0)
            Defeat();
    }

    #endregion

    #region Inherited Manipulators

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
