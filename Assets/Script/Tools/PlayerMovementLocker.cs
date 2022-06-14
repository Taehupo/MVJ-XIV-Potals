using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// lock / unlock player movements within a trigger
/// </summary>
public class PlayerMovementLocker : MonoBehaviour
{
    /// <summary>
    /// Delay after leaving the trigger for the character to stop
    /// </summary>
    public float StopDelay = 0.2f;

    /// <summary>
    /// Delay after stoping fo the character to respond again to control
    /// </summary>
    public float ResumeDelay = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player enter trigger lock Movement
        if (other.name == "Player")
        {
            CharacterManager.Instance.ShapeController.LockMovement();

            Debug.Log("yoyo enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if player exit trigger
        if (other.name == "Player")
        {
            CharacterManager.Instance.ShapeController.UnlockMovement(StopDelay, ResumeDelay);


            Debug.Log("yoyo leave");
        }
    }
}
