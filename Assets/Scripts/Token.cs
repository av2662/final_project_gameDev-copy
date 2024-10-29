using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public TokenSpawner spawner; // Reference to the CoinSpawner

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collected the token
        if (other.CompareTag("Player"))
        {
            // Notify the spawner to replace the collected token
            spawner.OnTokenCollected(gameObject);

            // Destroy this token after it’s collected
            Destroy(gameObject);
        }
    }
}
