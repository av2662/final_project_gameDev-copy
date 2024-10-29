using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pretty much the same logic i used for the ghosts
public class LadderSpawner : MonoBehaviour
{
    [SerializeField] GameObject ladderPrefab;                 
    [SerializeField] int initialLadderCountPerFloor = 2;  

    [SerializeField] float minDistanceBetweenLadders = 3f;     // Minimum distance between ladders

    // Define the X range for random spawn positions
    [SerializeField] float minX = -10f;
    [SerializeField] float maxX = 10f;

    // Define specific Y positions for each floor
    [SerializeField] float firstFloorY = -2f;
    [SerializeField] float secondFloorY = 3f;

    private List<GameObject> currentLadders = new List<GameObject>(); // Track active ladders

    void Start()
    {
        SpawnLaddersForFloor(firstFloorY, initialLadderCountPerFloor); // Spawn ladders on the first floor
        SpawnLaddersForFloor(secondFloorY, initialLadderCountPerFloor); // Spawn ladders on the second floor
    }

    // Method to spawn ladders on a specified floor
    void SpawnLaddersForFloor(float yPosition, int ladderCount)
    {
        for (int i = 0; i < ladderCount; i++)
        {
            SpawnLadder(yPosition);
        }
    }

    // Method to spawn a single ladder at a random position within the range for a specific floor
    void SpawnLadder(float yPosition)
    {
        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;

        // Try to find a valid spawn position
        while (!validPosition)
        {
            // Generate a random position within the specified X range
            float xPos = Random.Range(minX, maxX);
            spawnPosition = new Vector3(xPos, yPosition, 0);

            // Check if the new position is far enough from existing ladders
            validPosition = IsPositionValid(spawnPosition);
        }

        // Instantiate the ladder and add it to the currentLadders list
        GameObject newLadder = Instantiate(ladderPrefab, spawnPosition, Quaternion.identity);
        currentLadders.Add(newLadder);
    }

    // Method to check if a spawn position is valid
    bool IsPositionValid(Vector3 position)
    {
        foreach (GameObject ladder in currentLadders)
        {
            if (Vector3.Distance(ladder.transform.position, position) < minDistanceBetweenLadders)
            {
                return false; // Position is too close to an existing ladder
            }
        }
        return true; // Position is valid
    }
}
