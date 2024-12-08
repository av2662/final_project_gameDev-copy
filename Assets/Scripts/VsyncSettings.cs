
using UnityEngine;
using UnityEngine.UI;


public class VsyncSettings : MonoBehaviour
{
    [SerializeField] Toggle vsyncToggle;
    void Start()
    {
        // Set the toggle to the current V-Sync setting
            vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
            
            // Add listener to detect changes in the toggle
            vsyncToggle.onValueChanged.AddListener(OnVSyncToggleChanged);
    }
    void OnVSyncToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // Enable V-Sync 
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            // Disable V-Sync 
            QualitySettings.vSyncCount = 0;
        }
    }
}
