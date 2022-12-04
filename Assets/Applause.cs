using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Applause : MonoBehaviour
{
    public AudioClip sfx_applause;
    public AudioSource audioSource;
    public string playerTag = "Player";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sfx_applause, 0.6f);
        }
    }
}
