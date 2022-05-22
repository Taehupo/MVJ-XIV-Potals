using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour, IDamageable
{

    private bool hitRight = false;
    public bool HitRight { get => hitRight; set => hitRight = value; }

    public void TakeDamage(int damage)
    {
        // Destroy animation
        Debug.Log("Destroyed vase");
    }
}
