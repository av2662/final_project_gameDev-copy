using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class TokenCounter : MonoBehaviour
{
    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] TextMeshProUGUI tokenText;

    [SerializeField] int tokensToWinLevel;

    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        int countNum = playerInputHandler.GetPlayerCharacter().GetTokenCounter();
        String currentCount = countNum.ToString();
        tokenText.text = currentCount + (" / ") + tokensToWinLevel;
        if (countNum >= tokensToWinLevel)
        {
            SceneManager.LoadScene("Level2Scene");
        }
        //this needs to be fixed for other levels
        //tokenText.text = playerInputHandler.GetPlayerCharacter().GetTokenCounter().ToString();
    }
}
