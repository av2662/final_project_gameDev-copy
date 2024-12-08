using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class IlluminateController : MonoBehaviour
{
    public Light2D globalLight;  
    public Button illuminateButton; // Reference to the UI button
    public float illuminationDuration = 8f; // How long the illumination lasts
    public float cooldownDuration = 15f;   
    public float brightIntensity = 1f;    
    public float dimIntensity = 0.1f;     // Normal brightness

    private bool isCooldown = false;

    void Start()
    {
        // Attach the button click event
        illuminateButton.onClick.AddListener(OnIlluminateButtonClicked);
        ResetButtonState();
    }

    void OnIlluminateButtonClicked()
    {
        if (!isCooldown)
        {
            StartCoroutine(IlluminateScene());
        }
    }

    IEnumerator IlluminateScene()
    {
        // Brighten the global light
        globalLight.intensity = brightIntensity;

        // Disable the button during illumination
        illuminateButton.interactable = false;

        
        yield return new WaitForSeconds(illuminationDuration);

        // Dim the global
        globalLight.intensity = dimIntensity;

        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;

        
        yield return new WaitForSeconds(cooldownDuration);

        isCooldown = false;
        ResetButtonState();
    }

    void ResetButtonState()
    {
        
        illuminateButton.interactable = true;
    }
}
