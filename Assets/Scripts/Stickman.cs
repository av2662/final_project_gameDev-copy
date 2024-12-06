using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Stickman : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 5;

    [SerializeField] AudioSource audioSource;

    public bool IsOnStairs = false;
    [SerializeField] bool isClimbing = false;

    [SerializeField] Image[] heartIcons;               // Array for heart symbols
    [SerializeField] Color caughtColor = Color.gray;   // Color for heart when caught
    //private GhostAI ghostAI;
    
    Vector3 climbingDirection;
    int tokenCount = 0;
    int caught = 0;

    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
   

    void Update()
    {
        if (isClimbing)
        {
            // Move the player in the climbing direction
            transform.localPosition += climbingDirection * speed * Time.deltaTime;
        }
    }
    public void Move(Vector3 movement)
    {
            transform.localPosition += movement * speed * Time.deltaTime;
            
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
            caught++;
            Debug.Log("ghost collision");
            UpdateHeartIcons(); // Call to update hearts
            // Get the GhostAI component and call CatchPlayer
            GhostAI ghostAI = other.GetComponent<GhostAI>();
            if (ghostAI != null)
            {
                ghostAI.CatchPlayer(); // Call the instance method
            }
          
            //change color here
            if(caught > 1){
              Debug.Log("GAME OVER");  
              SceneManager.LoadScene("MainMenu"); 
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
}
