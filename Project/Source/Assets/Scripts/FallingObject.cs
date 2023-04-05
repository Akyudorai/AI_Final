using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public AudioSource source;
    public AudioClip boop1, boop2, splat;
    public void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.name == "Peg")
        {
            int random = Random.Range(0, 2);
            source.PlayOneShot((random == 0) ? boop1 : boop2, 0.5f);
        }
    }
}
