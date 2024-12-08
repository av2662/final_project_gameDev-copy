using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Image moveImage;

    public void StartGame(){

        StartCoroutine(TransitionInAndStartGame());
    }
    IEnumerator TransitionInAndStartGame()
    {
        Vector3 startPosition = moveImage.transform.localPosition;
        Vector3 targetPosition = new Vector3(0, startPosition.y, startPosition.z);
        Vector3 movement = Vector3.zero;

        while(moveImage.transform.localPosition.x > targetPosition.x){
            movement += new Vector3(-1,0,0);
            moveImage.transform.localPosition += movement * 2 * Time.deltaTime;
            yield return null;
        }
         if (PlayerPrefs.HasKey("SavedLevel"))
        {
            string savedLevel = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(savedLevel); 
        }
        else
        {
            SceneManager.LoadScene("Level1Scene"); 
        }
    }
    public void QuitGame(){
        Application.Quit();
    }
    
}
