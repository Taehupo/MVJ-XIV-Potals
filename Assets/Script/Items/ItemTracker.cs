using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] GameObject item;
    public float speed = 20.0f; 
    private bool active = false;
    private Timer timer;

    private void Awake() {
        timer = gameObject.AddComponent<Timer>();
        timer.OnEnd = () => { active = true; };
        timer.StartTimer(0.5f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && active)
        {
            Vector3 playerPosition = collision.gameObject.transform.position;
            
            item.GetComponent<Floater>().posOffset = Vector3.MoveTowards(item.GetComponent<Floater>().posOffset, playerPosition, speed*Time.deltaTime);
            item.transform.position = item.GetComponent<Floater>().posOffset;
        }
    }
}
