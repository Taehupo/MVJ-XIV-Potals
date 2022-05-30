using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region Members
    private bool hitRight = false;
    private bool isInvincible = false;
    public Timer invincibleTimer;
    public Action onHurt;
    public Action onDefeat;

    [SerializeField]
    private int maxHealth = 1;
    private int health = 1;
    #endregion

    #region Public Manipulators
    public bool IsAlive() { return health > 0; }
    public bool IsInvincible() { return isInvincible; }
    public void StopInvincibility() { isInvincible = false; }
    public void Heal(int healValue) { health = Math.Min(maxHealth, health+healValue); }
    public void SetMaxHealth(int _maxHealth) { maxHealth = _maxHealth; health = maxHealth; }
    public void TakeHit(int damage, GameObject striker)
    {
        this.TakeDamage(damage);
        this.SetHitLocation(striker, this.gameObject);
    }
    public void TakeDamage(int damage)
    {
        if (IsAlive() && !isInvincible)
        {
            health = Math.Max(health-damage,0);
            // Debug.Log("Took " + damage + " Health : " + health + "/" + maxHealth);
            isInvincible = true;
            invincibleTimer.StartTimer(1);
            onHurt?.Invoke();
        }

        if (health == 0)
        {
            // Debug.Log("Dead");
            onDefeat?.Invoke();
        }
    }
    public int GetHitLocation()
    {
        return (hitRight ? 1 : -1);
    }
    public void SetHitLocation(GameObject striker, GameObject striked)
    {
        hitRight = (striker.transform.position.x < striked.transform.position.x);
    }

    #endregion

    #region Inherited Manipulators

    #endregion

    #region Private Manipulators

    private void Start()
    {
    }

    private void Awake()
    {
        health = maxHealth;
        invincibleTimer = gameObject.AddComponent<Timer>();
    }

    #endregion

}
