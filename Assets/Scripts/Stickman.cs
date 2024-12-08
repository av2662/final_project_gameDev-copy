using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Stickman : MonoBehaviour
{
    Rigidbody2D rb;
     public GameObject flashlight;  // Reference to the flashlight object
    [SerializeField] float speed = 5;
    bool facingRight = true; // Track the facing direction
    bool isHidden = false;

    
    [SerializeField] Image loseGameImage;
    

    public bool IsOnStairs = false;
    [SerializeField] bool isClimbing = false;

    [SerializeField] Image[] heartIcons;               // Array for heart symbols
    [SerializeField] Color caughtColor = Color.gray;   // Color for heart when caught
    //private GhostAI ghostAI;
    
    Vector3 climbingDirection;
    int tokenCount = 0;
    int caught = 0;

    float minX, maxX, minY, maxY;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
         Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minX = bottomLeft.x + 0.5f; 
        maxX = topRight.x - 0.5f;
        minY = bottomLeft.y + 0.5f;
        maxY = topRight.y - 0.5f;
       
    }

    void Update()
    {
        if (isClimbing)
        {
            // Move the player in the climbing direction
            transform.localPosition += climbingDirection * speed * Time.deltaTime;
        }
        if (isHidden)
        {
            TurnOffFlashlight(); // Turn off the flashlight if hidden
        }
        else
        {
            TurnOnFlashlight(); // Turn on the flashlight if not hidden
        }
    }

    public void Move(Vector3 movement)
    {
        // Calculate the new position
        Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;

        // Clamp the position to not go off screen
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Apply the clamped position
        transform.position = newPosition;

        // Flip the stickman based on movement direction
        if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && facingRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        facingRight = !facingRight; // Toggle facing direction
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the sprite horizontally
        transform.localScale = scale;
    }


    public void StartClimbing(bool isClimbingUp)
    {
        if (IsOnStairs)
        {
            isClimbing = true;
            climbingDirection = isClimbingUp ? Vector3.up : Vector3.down;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StairsTrigger"))
        {
            IsOnStairs = true;
            //Debug.Log("Entered stairs area");
        }
        if(other.CompareTag("Ghost_Collision")){
            if(isHidden){
                return;
            }
            caught++;
            Debug.Log("ghost collision");
            UpdateHeartIcons(); // Call to update hearts
            // Get the GhostAI component and call CatchPlayer
           /* GhostAI ghostAI = other.GetComponent<GhostAI>();
            if (ghostAI != null)
            {
                ghostAI.CatchPlayer(); // Call the instance method
            }
            */
            TriggerAggressiveMode();
            //change color here
            if(caught > 1){
              Debug.Log("GAME OVER");  
               StartCoroutine(TransitionInLose());
              //SceneManager.LoadScene("MainMenu"); 
            } 
        }
         if(other.CompareTag("Elevator")){
            Debug.Log("ELEVATOR HELLO");
         }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StairsTrigger"))
        {
            IsOnStairs = false;
            isClimbing = false;
           // Debug.Log("Exited stairs area");
        }
    }
    public int GetTokenCounter(){
        return tokenCount;
    }
    public void setTokenCount(int token){
        tokenCount = token;
    }
    // Method to update heart colors based on lives left
    void UpdateHeartIcons()
    {
        // Change the color of each heart based on the caught count
        for (int i = 0; i < caught && i < heartIcons.Length; i++)
        {
            heartIcons[i].color = caughtColor;
        }
    }
    // Trigger all ghosts to enter aggressive mode
    void TriggerAggressiveMode()
    {
        GhostAI[] ghosts = FindObjectsOfType<GhostAI>(); // Find all ghosts in the scene
        foreach (GhostAI ghost in ghosts)
        {
            ghost.CatchPlayer();  // Call the method to set each ghost to aggressive mode
        }
    }
    public void SetHidden(bool hidden)
    {
        isHidden = hidden;

    }

    public bool IsHidden()
    {
        return isHidden;
    }

    // Method to turn off the flashlight
    private void TurnOffFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.SetActive(false);  // Disable the flashlight
        }
    }

    // Method to turn on the flashlight
    private void TurnOnFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.SetActive(true);  // Enable the flashlight
        }
    }
    IEnumerator TransitionInLose()
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
        Vector3 startPosition = loseGameImage.transform.localPosition;
        Vector3 targetPosition = new Vector3(0, startPosition.y, startPosition.z);
        Vector3 movement = Vector3.zero;

        while(loseGameImage.transform.localPosition.x > targetPosition.x){
            movement += new Vector3(-1,0,0);
            loseGameImage.transform.localPosition += movement * 2 * Time.deltaTime;
            yield return null;
        }
        // Wait for 5 seconds in real time
        yield return new WaitForSecondsRealtime(3f);

        // Delete the "SavedLevel" PlayerPrefs key if it exists
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            PlayerPrefs.DeleteKey("SavedLevel");
            Debug.Log("Deleted 'SavedLevel' PlayerPrefs key.");
        }
        SceneManager.LoadScene("MainMenu");

    }
}