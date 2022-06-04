using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSprings : MonoBehaviour
{
    // keep a copy of the executing script
    private IEnumerator coroutine;

    [SerializeField]
    private float waitTime = 1f;

    [SerializeField]
    private int healAmount = 1;
    [SerializeField]
    private int ammoAmount = 2;

    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartHealing(waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player set vcamera priority
        if (other.name == "Player")
        {
            StartCoroutine(coroutine);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if player reset vcamera priority
        if (other.name == "Player")
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator StartHealing(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            //print("WaitAndPrint " + Time.time);
            CharacterManager.Instance.HealthManager.Heal(healAmount);
            CharacterManager.Instance.AddAmmo(ammoAmount);
        }
    }
}
