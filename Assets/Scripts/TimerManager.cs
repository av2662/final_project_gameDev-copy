using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float totalTime = 60f; // Total time in seconds
    [SerializeField] TextMeshProUGUI timerText;       // Reference to the UI Text element

    [SerializeField] Image loseGameImage;
    private float remainingTime;
    private bool isTimerRunning = true;

    void Start()
    {
        remainingTime = totalTime;
        UpdateTimerUI();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isTimerRunning = false;
                HandleTimeUp(); // Trigger the win/loss condition
            }

            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void HandleTimeUp()
    {
        Debug.Log("Time's up!");
        StartCoroutine(TransitionInLose());
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

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }
    
}
