using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    #region Members

    [SerializeField] private int maxHealth;
    private HealthManager _healthManager;
    private SpriteManager _spriteManager;

    protected bool CanBeStaggered;
    private bool _isStaggered = false;
    private Rigidbody2D _rigidbody2D;

    #endregion

    #region Public Manipulators


    #endregion

    #region Inherited Manipulators

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _healthManager = gameObject.AddComponent<HealthManager>();
        _healthManager.SetMaxHealth(maxHealth);
        _healthManager.onHurt += Hurt;
        _healthManager.onDefeat += Defeat;
        _healthManager.invincibleTimer.OnEnd = () => { _healthManager.StopInvincibility(); _spriteManager.StopBlink(); };
        
        _spriteManager = gameObject.AddComponent<SpriteManager>();
    }

    private void FixedUpdate()
    {
        if (_isStaggered)
        {
            _rigidbody2D.velocity = new Vector2(_healthManager.GetHitLocation()*10f, 10f);
            _isStaggered = false;
        }
    }

    #endregion

    #region Private Manipulators

    protected virtual void Defeat()
    {
    }
    private void Hurt()
    {
        _isStaggered = true;
        _spriteManager.Blink();
    }

    #endregion

}
