using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class IlluminateController : MonoBehaviour
{
    public Light2D globalLight;  // Reference to the Global Light 2D
    public Button illuminateButton; // Reference to the UI button
    public float illuminationDuration = 8f; // How long the illumination lasts
    public float cooldownDuration = 15f;   // Cooldown time between uses
    public float brightIntensity = 1f;    // Brightness during illumination
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

        // Wait for the illumination duration
        yield return new WaitForSeconds(illuminationDuration);

        // Dim the global light back to normal
        globalLight.intensity = dimIntensity;

        // Start cooldown
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(cooldownDuration);

        isCooldown = false;
        ResetButtonState();
    }

    void ResetButtonState()
    {
        // Enable the button and reset any UI changes
        illuminateButton.interactable = true;
    }
}
