using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilumDoor : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("coll");
        if (collision.otherCollider.GetComponent<Javelin>() != null)
            GetComponent<HealthManager>().TakeDamage(1);
    }
    public void Hit()
    {
        Destroy(gameObject);
    }
}
