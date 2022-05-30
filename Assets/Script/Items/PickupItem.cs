using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class PickupItem : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            if (collision.GetComponent<HealthManager>() != null)
            {
                Debug.Log("Touched " + collision.name);
                Effect(collision);
                RemoveItem();
            }
        }
    }
    public abstract void Effect(Collider2D collision);

    private void RemoveItem()
    {
        Destroy(gameObject);
    }
}
