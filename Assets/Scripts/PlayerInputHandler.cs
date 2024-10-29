using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] Stickman stickman;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        // Start climbing if the player presses "W" or "S" while on stairs
        if (stickman.IsOnStairs)
        {
            if (Input.GetKeyDown(KeyCode.W))  // Trigger climb when "W" is pressed
            {
                stickman.StartClimbing(true);  // Climb up
            }
            else if (Input.GetKeyDown(KeyCode.S))  // Trigger descent when "S" is pressed
            {
                stickman.StartClimbing(false); // Climb down
            }
        }
    }

    void FixedUpdate(){
        Vector3 movement = Vector3.zero;

        if(Input.GetKey(KeyCode.A)){
            movement += new Vector3(-1, 0, 0); //left
        }
        if(Input.GetKey(KeyCode.D)){
            movement += new Vector3(1, 0, 0); //rigt
        }
        
        stickman.Move(movement);
    }
    public Stickman GetPlayerCharacter(){
        return stickman;
    }
}

