using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        // Destroy animation
        Debug.Log("Destroyed vase");
    }
}
