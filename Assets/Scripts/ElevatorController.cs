using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{
    public Transform topFloorPosition;  // The position the elevator should move to
    public int coinsRequired = 10;     // Number of coins required to activate the elevator
    public float moveSpeed = 2f;       // Speed at which the elevator moves
    private bool isMoving = false;     // Track if the elevator is already moving

    public Stickman player;            // Reference to the Stickman script
    private Transform playerTransform; // Reference to the player's Transform

    void Start()
    {
        // Get the player's Transform at the start
        playerTransform = player.transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the elevator is triggered by the player
        if (other.CompareTag("Player") && player.GetTokenCounter() >= coinsRequired && !isMoving)
        {
            StartCoroutine(MoveElevator());
        }
    }

    IEnumerator MoveElevator()
    {
        isMoving = true;

        // Parent the player to the elevator to move together
        if (playerTransform != null)
        {
            playerTransform.SetParent(transform);
        }

        // Animate the elevator moving upwards
        while (Vector3.Distance(transform.position, topFloorPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, topFloorPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Pause briefly before transitioning levels
        yield return new WaitForSeconds(1f);

        // Unparent the player after reaching the destination
        if (playerTransform != null)
        {
            playerTransform.SetParent(null);
        }

        // Load the next scene
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
