using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSpawner : MonoBehaviour
{
    [SerializeField] GameObject windowPrefab; // Prefab for the window
    [SerializeField] private List<GameObject> ghosts; // List of ghost GameObjects
    [SerializeField] private float offsetX = 1.5f;    // Horizontal offset from the ghost's spawn position
     private float offsetY = 2f;      // Vertical offset from the ghost's spawn position

    void Start()
    {
        StartCoroutine(DelayedSpawnWindows());
    }

    IEnumerator DelayedSpawnWindows()
    {
        yield return new WaitForSeconds(.3f); // Wait for 1 second to allow ghosts to spawn
        ghosts = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ghost_Collision"));

        if (ghosts == null || ghosts.Count == 0)
        {
            Debug.LogError("No ghosts found with the tag 'Ghost_Collision'. Windows will not spawn.");
            yield break;
        }

        Debug.Log($"Found {ghosts.Count} ghosts after delay. Spawning windows...");
        SpawnWindowsNearGhosts();
    }
    void SpawnWindowsNearGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            if (ghost != null)
            {
                // Get the ghost's initial spawn position
                Vector3 ghostPosition = ghost.transform.position;

                // Calculate the window spawn position
                Vector3 windowPosition = new Vector3(ghostPosition.x + offsetX, ghostPosition.y + offsetY, ghostPosition.z);

                // Instantiate the window at the calculated position
                Instantiate(windowPrefab, windowPosition, Quaternion.identity);
            }
        }
    }
}
