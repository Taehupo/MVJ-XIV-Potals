using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    #region Members

    [SerializeField] private int maxHealth;
    protected HealthManager _healthManager { get; private set; }
    protected SpriteManager _spriteManager { get; private set; }

    protected bool CanBeStaggered;
    protected bool IsStaggered = false;
    protected Rigidbody2D Rigidbody2D;
    [SerializeField] protected GameObject attackHitbox;
    
    [SerializeField] private LayerMask playerLayer;
    protected ContactFilter2D ContactFilter = new();
    [SerializeField] protected int hitDamage;

    #endregion

    #region Public Manipulators


    #endregion

    #region Inherited Manipulators

    // Start is called before the first frame update
    private void Start()
    {
        ContactFilter.SetLayerMask(playerLayer);
        
        Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _healthManager = gameObject.AddComponent<HealthManager>();
        _healthManager.invincibleTime = 0.75f;
        _healthManager.SetMaxHealth(maxHealth);
        _healthManager.onHurt += Hurt;
        _healthManager.onDefeat += Defeat;
        _healthManager.invincibleTimer.OnEnd = () => { _healthManager.StopInvincibility(); _spriteManager.StopBlink(); IsStaggered = false; };
        
        _spriteManager = gameObject.AddComponent<SpriteManager>();
    }

    private void FixedUpdate()
    {
        // int test = Random.Range(0, 100);
        // if (test > 95)
        //     this.Attack();
    }

    #endregion

    #region Private Manipulators
    protected abstract void Attack();

    protected virtual void Defeat()
    {
    }
    private void Hurt()
    {
        IsStaggered = true;
        _spriteManager.Blink();
    }

    #endregion

}
