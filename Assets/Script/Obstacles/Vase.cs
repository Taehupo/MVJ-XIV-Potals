using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    private HealthManager healthManager;
    void Awake()
    {
        healthManager = gameObject.AddComponent<HealthManager>();
        healthManager.invincibleTimer.OnEnd = () => { healthManager.StopInvincibility(); };
        healthManager.onHurt += Hurt;
        healthManager.onDefeat += Defeat;
    }
    private void Hurt()
    {

    }
    private void Defeat()
    {
        // Destroy animation
        Debug.Log("Destroyed vase");
    }
}
