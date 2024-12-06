/*using System.Collections;
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
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    // Patrol settings
    [SerializeField] private float patrolSpeed = 2f;
    private bool movingRight = true;
    private Rigidbody2D rb;
    private Vector3 startingPosition;
    private float patrolDistance = 3f; // Distance to move left or right

    // Player reference
    private Transform player;

    // State machine
    private IGhostState currentState;

    // Behavior changes
    private bool playerCaught = false;
    [SerializeField] private float increasedSpeed = 4f;
    [SerializeField] private Color caughtColor = Color.red;
    [SerializeField] private Vector3 increasedScale = new Vector3(1.5f, 1.5f, 1f);

    // Components
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingPosition = transform.position; // Record initial position as the patrol center

        // Find the player by tag at the start
        player = GameObject.FindWithTag("Player").transform;

        // Initialize with patrol state
        currentState = new PatrolState();
        currentState.EnterState(this);
    }

    void Update()
    {
        if (player == null) return; // Error check to see if we found the player

        // Execute the current state's logic
        currentState.UpdateState(this);
    }

    // State Interface
    public interface IGhostState
    {
        void EnterState(GhostAI ghost);
        void UpdateState(GhostAI ghost);
    }

    // Patrol State
    public class PatrolState : IGhostState
    {
        public void EnterState(GhostAI ghost)
        {
            Debug.Log("Ghost entered Patrol State.");
        }

        public void UpdateState(GhostAI ghost)
        {
            ghost.Patrol();
        }
    }

    // Aggressive State
    public class AggressiveState : IGhostState
    {
        public void EnterState(GhostAI ghost)
        {
            Debug.Log("Ghost entered Aggressive State.");

            // Change behavior when in aggressive state
            ghost.patrolSpeed = ghost.increasedSpeed;
            ghost.spriteRenderer.color = ghost.caughtColor;
            ghost.transform.localScale = ghost.increasedScale;
        }

        public void UpdateState(GhostAI ghost)
        {
            ghost.Patrol(); // Continue patrolling, but faster and with updated visuals
        }
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

    public void CatchPlayer()
    {
        if (!playerCaught)
        {
            playerCaught = true;

            // Transition to Aggressive state
            currentState = new AggressiveState();
            currentState.EnterState(this);
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
