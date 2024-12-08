using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;                  // Ghost prefab to spawn
    //[SerializeField] int initialGhostCountPerFloor = 2;       // Number of ghosts to spawn on each floor
    
    [SerializeField] Transform player;                        // Reference to the player
    [SerializeField] float minDistanceFromPlayer = 2f;   
    [SerializeField] int ghostCountFloor1 = 1;       // Number of ghosts to spawn on each floor
    
    [SerializeField] int ghostCountFloor2 = 1;
    [SerializeField] int ghostCountFloor3 = 1;
    [SerializeField] float minDistanceBetweenGhosts = 3f;     // Minimum distance between ghosts

    // Defines the X range for random spawn positions
    [SerializeField] float minX = -10f;
    [SerializeField] float maxX = 10f;

    // Defines specific Y positions for each floor
    [SerializeField] float firstFloorY = -2f;
    [SerializeField] float secondFloorY = 3f;

     [SerializeField] float thirdFloorY = 3f;
    


    private List<GameObject> currentGhosts = new List<GameObject>(); // Track active ghosts

    void Start()
    {
        SpawnGhostsForFloor(firstFloorY, ghostCountFloor1); 
        SpawnGhostsForFloor(secondFloorY, ghostCountFloor2); 
        SpawnGhostsForFloor(thirdFloorY, ghostCountFloor3); 
    }

    // Method to spawn ghosts on a specified floor
    void SpawnGhostsForFloor(float yPosition, int ghostCount)
    {
        for (int i = 0; i < ghostCount; i++)
        {
            SpawnGhost(yPosition);
        }
    }

    // spawn a single ghost at random position
    void SpawnGhost(float yPosition)
    {
        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;

        // find a valid position
        while (!validPosition)
        {
            
            float xPos = Random.Range(minX, maxX);
            spawnPosition = new Vector3(xPos, yPosition, 0);

            // Check if the new position is far enough from the ghosts already established in the game
            validPosition = IsPositionValid(spawnPosition);
        }

        // Instantiate the ghost and add it to the currentGhosts list
        GameObject newGhost = Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
        currentGhosts.Add(newGhost);
    }

    // check if a spawn position is valid
  
    bool IsPositionValid(Vector3 position)
    {
        // Check distance from the player
        if (Vector3.Distance(player.position, position) < minDistanceFromPlayer)
        {
            return false; // Position is too close to the player
        }

        // Check distance from existing ghosts
        foreach (GameObject ghost in currentGhosts)
        {
            if (Vector3.Distance(ghost.transform.position, position) < minDistanceBetweenGhosts)
            {
                return false; // Position is too close to an existing ghost
            }
        }

        return true; // Position is valid
    }
}
