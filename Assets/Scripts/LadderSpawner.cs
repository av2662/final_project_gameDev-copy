using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSpawner : MonoBehaviour
{
    [SerializeField] GameObject ladderPrefab;
    [SerializeField] int initialLadderCountPerFloor = 2;

    [SerializeField] float minDistanceBetweenLadders = 3f; // Minimum distance between ladders

   
    [SerializeField] float minX = -10f;
    [SerializeField] float maxX = 10f;

  
    [SerializeField] float firstFloorY = -2f;
    [SerializeField] float secondFloorY = 3f;

    [SerializeField] Transform elevatorPosition; // Reference to the elevator position
    [SerializeField] float elevatorAvoidanceRadius = 3f; // Radius around the elevator to avoid placing ladders

    private List<GameObject> currentLadders = new List<GameObject>(); // Track active ladders

    void Start()
    {
        SpawnLaddersForFloor(firstFloorY, initialLadderCountPerFloor); // Spawn ladders on the first floor
        SpawnLaddersForFloor(secondFloorY, initialLadderCountPerFloor); // Spawn ladders on the second floor
    }

   
    void SpawnLaddersForFloor(float yPosition, int ladderCount)
    {
        for (int i = 0; i < ladderCount; i++)
        {
            SpawnLadder(yPosition);
        }
    }

    // spawn a single ladder at a random position within the range for a specific floor
    void SpawnLadder(float yPosition)
    {
        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;

       
        while (!validPosition)
        {
            
            float xPos = Random.Range(minX, maxX);
            spawnPosition = new Vector3(xPos, yPosition, 0);

            // Check if the new position is valid
            validPosition = IsPositionValid(spawnPosition);
        }

       
        GameObject newLadder = Instantiate(ladderPrefab, spawnPosition, Quaternion.identity);
        currentLadders.Add(newLadder);
    }

    
    bool IsPositionValid(Vector3 position)
{
    // Check distance from existing ladders
    foreach (GameObject ladder in currentLadders)
    {
        if (Vector3.Distance(ladder.transform.position, position) < minDistanceBetweenLadders)
        {
            return false; // Position is too close to an existing ladder
        }
    }

    // Check horizontal distance from the elevator
    if (elevatorPosition != null)
    {
        float xDistanceToElevator = Mathf.Abs(position.x - elevatorPosition.position.x);
        if (xDistanceToElevator < elevatorAvoidanceRadius) // Check x-distance only
        {
            return false; // Position is too close to the elevator's x-position
        }
    }

    return true; // Position is valid
}

}
