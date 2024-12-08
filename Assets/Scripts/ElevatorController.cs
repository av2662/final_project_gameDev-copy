using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ElevatorController : MonoBehaviour
{
    public float[] floorPositions;      // Array of valid Y positions for the floors
    public float moveSpeed = 2f;        // Speed at which the elevator moves
    int currentFloor = 0;       // Tracks the current floor index
    bool isMoving = false;      // Prevent simultaneous movements

    Transform playerTransform;  // Tracks the player riding the elevator
    [SerializeField] Stickman player;            // Reference to the Stickman script
    [SerializeField] int coinsRequired = 10;     // Number of coins required to activate the elevator

    [SerializeField] Image nextLevelImage;

    public bool IsPlayerOnElevator(Stickman stickman)
    {
        return playerTransform != null && playerTransform.GetComponent<Stickman>() == stickman;
    }

    // Cooldown settings
    public float cooldownTime = 3f;   // Time in seconds before the elevator can be used again
    private bool isCooldown = false;  // 
    private IElevatorState currentState;  

   
    public AudioSource elevatorAudioSource;
    public AudioClip readySound;

    
    public SpriteRenderer elevatorSpriteRenderer;
    public Color cooldownColor = Color.red;
    private Color originalColor;

    private void Start()
    {
        originalColor = elevatorSpriteRenderer.color;
        currentState = new ReadyState(); // Initial state
        currentState.EnterState(this);
    }

    public void MoveElevator(int direction)
    {
        if (isMoving || isCooldown) return;  // Prevent movement if elevator is moving or in cooldown

        int targetFloor = currentFloor + direction;

        // Check if the target floor is valid
        if (targetFloor >= 0 && targetFloor < floorPositions.Length)
        {
            StartCoroutine(MoveToFloor(targetFloor));  
        }
        else
        {
            Debug.Log("Cannot move further in that direction.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;  // Reference the player entering the elevator
            if(player.GetTokenCounter() >= coinsRequired){
                 StartCoroutine(TransitionInNextLevel());
            }
        }
    }
    IEnumerator TransitionInNextLevel()
    {
       // Disable player controls and any gameplay logic
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("jdlfkjsl");
            player.GetComponent<Stickman>().enabled = false; 
        }

       
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost_Collision");
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<GhostAI>().enabled = false; 
        }
        Vector3 startPosition = nextLevelImage.transform.localPosition;
        Vector3 targetPosition = new Vector3(0, startPosition.y, startPosition.z);
        Vector3 movement = Vector3.zero;

        while(nextLevelImage.transform.localPosition.x > targetPosition.x){
            movement += new Vector3(-1,0,0);
            nextLevelImage.transform.localPosition += movement * 2 * Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSecondsRealtime(3f);
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;  
        }
    }

    private IEnumerator MoveToFloor(int floorIndex)
    {
        isMoving = true;
        isCooldown = true;  // Start the cooldown after elevator is activated
        float targetY = floorPositions[floorIndex];

        if (playerTransform != null)
        {
            playerTransform.SetParent(transform);
            playerTransform.GetComponent<Stickman>().enabled = false;  // Disable player movement script
        }

       
        while (Mathf.Abs(transform.position.y - targetY) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     new Vector3(transform.position.x, targetY, transform.position.z), 
                                                     moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Set the elevator to the target floor position
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        currentFloor = floorIndex;  // Update the current floor index

        // Unparent the player after reaching the destination
        if (playerTransform != null)
        {
            playerTransform.SetParent(null);
             playerTransform.GetComponent<Stickman>().enabled = true;
        }

        isMoving = false;  // Allow for new movement after reaching the target
        currentState.ExitState(this);
        currentState = new CooldownState();
        currentState.EnterState(this);  // Transition back to the ready state

        // Wait for the cooldown before allowing the elevator to be used again
        yield return new WaitForSeconds(cooldownTime);

        isCooldown = false;  // Cooldown is over, elevator can be used again
        currentState.ExitState(this);
        currentState = new ReadyState();
        currentState.EnterState(this);  // Transition back to the ready state
    }

    // State Interface
    public interface IElevatorState
    {
        void EnterState(ElevatorController elevator);
        void ExitState(ElevatorController elevator);
       
    }

    // Ready State
    public class ReadyState : IElevatorState
    {
        public void EnterState(ElevatorController elevator)
        {
            if (elevator.elevatorAudioSource != null && elevator.readySound != null)
            {
                elevator.elevatorAudioSource.clip = elevator.readySound;
                elevator.elevatorAudioSource.Play();
            }

            elevator.elevatorSpriteRenderer.color = elevator.originalColor;  // Reset the color to original
        }

        public void ExitState(ElevatorController elevator)
        {
            // No special exit behavior for Ready state
        }

       
    }

    // Cooldown State
    public class CooldownState : IElevatorState
    {
        public void EnterState(ElevatorController elevator)
        {
            elevator.elevatorSpriteRenderer.color = elevator.cooldownColor;  // Change the color to cooldown color
        }

        public void ExitState(ElevatorController elevator)
        {
            // No special exit behavior for Cooldown state
        }

    }
}
