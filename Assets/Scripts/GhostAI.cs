using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [SerializeField] float patrolSpeed = 2f;
    //[SerializeField] float detectionRange = 5f;

    Transform player; // Private reference to the player object
    bool movingRight = true;
    Rigidbody2D rb;
    Vector3 startingPosition;
    float patrolDistance = 3f; // Distance to move left or right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position; // Record initial position as the patrol center
        
        // Find the player by tag at the start
        player = GameObject.FindWithTag("Player").transform; 
    }

    void Update()
    {
        if (player == null) return; //error check to see if we found the player

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        Patrol();
        
    }

    void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            if (transform.position.x >= startingPosition.x + patrolDistance)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
            if (transform.position.x <= startingPosition.x - patrolDistance)
            {
                movingRight = true;
                Flip();
            }
        }
    }
    void IncreaseSpeed()
    {
        
    }
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
