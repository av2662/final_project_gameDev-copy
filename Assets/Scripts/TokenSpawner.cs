using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TokenSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] tokens;               // Array to store different token prefabs
    [SerializeField] int initialCoinCount = 5;          // Number of coins to spawn at the beginning

    public float minDistanceBetweenTokens = 2f; // Minimum distance between tokens

    [SerializeField] PlayerInputHandler playerInputHandler;

    [SerializeField] float[] yPositions;
    // Define the range for random spawn positions
    [SerializeField]float minX = -10f;
    [SerializeField] float maxX = 10f;
  

    private List<GameObject> currentTokens = new List<GameObject>(); // Track active coins

    int tokenCount = 0;

    void Start()
    {
        SpawnInitialCoins();                  // Spawn initial set of coins at the start of the level
    }

    // Method to spawn initial coins
    void SpawnInitialCoins()
    {
        for (int i = 0; i < initialCoinCount; i++)
        {
            SpawnToken();
        }
    }

    // spawn a single token at a random position within the range
    public void SpawnToken()
    {
        // Select a random token prefab
        int tokenIndex = Random.Range(0, tokens.Length);
        GameObject selectedToken = tokens[tokenIndex];

        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;

        //find a valid spawn position
        while (!validPosition)
        {
           
            float xPos = Random.Range(minX, maxX);
            float yPos = yPositions[Random.Range(0, yPositions.Length)];
            spawnPosition = new Vector3(xPos, yPos, 0);

            // Check if far enough from existing tokens
            validPosition = IsPositionValid(spawnPosition);
        }

        // Instantiate the token and add it to the currentTokens list
        GameObject newToken = Instantiate(selectedToken, spawnPosition, Quaternion.identity);
        currentTokens.Add(newToken);

        // Assign the TokenSpawner as a reference in the token's script for respawn
        Token tokenScript = newToken.GetComponent<Token>();
        tokenScript.spawner = this;
    }

    // check if a spawn position is valid
    bool IsPositionValid(Vector3 position)
    {
        foreach (GameObject token in currentTokens)
        {
            if (Vector3.Distance(token.transform.position, position) < minDistanceBetweenTokens)
            {
                return false; // too close to existing token
            }
        }
        return true;
    }

    // Method to handle a token being collected
    public void OnTokenCollected(GameObject collectedToken)
    {
        // Increase score based on the tag of the collected token
        if (collectedToken.CompareTag("token5"))
            tokenCount += 5;
        else if (collectedToken.CompareTag("token10"))
            tokenCount += 10;
        else if (collectedToken.CompareTag("token15"))
            tokenCount += 15;
        else if (collectedToken.CompareTag("token30"))
            tokenCount += 30;

        currentTokens.Remove(collectedToken);  // Remove from active token list
        Destroy(collectedToken);               // Destroy the collected token

        playerInputHandler.GetPlayerCharacter().setTokenCount(tokenCount);
        SpawnToken();                          // Spawn a replacement token
    }
}