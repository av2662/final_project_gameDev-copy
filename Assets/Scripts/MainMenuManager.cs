using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame(){

        SceneManager.LoadScene("Level1Scene");
        
    }
    public void QuitGame(){
        Application.Quit();
    }
}
