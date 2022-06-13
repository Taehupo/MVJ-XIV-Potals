using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string saveAnimationName = "Saving";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player set vcamera priority
        if (other.name == "Player")
        {
            GameManager.instance.SaveGame();
            // Check animation to avoid playing twice
            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName(saveAnimationName))
            {
                animator.Play(saveAnimationName);
            }
        }
    }

    /*private void OnTriggerExit2D(Collider2D other)
    {
        // if player reset vcamera priority
        if (other.name == "Player")
        {
            GameManager.instance.GetComponent<SaveSystem>().ReadFile();
        }
    }*/
}
