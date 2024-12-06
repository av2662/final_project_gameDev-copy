using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame(){

        // Check if a saved level exists
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            string savedLevel = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(savedLevel); // Load the saved level
        }
        else
        {
            SceneManager.LoadScene("Level1Scene"); // Default starting level
        }
        
    }
    public void QuitGame(){
        Application.Quit();
    }
}
