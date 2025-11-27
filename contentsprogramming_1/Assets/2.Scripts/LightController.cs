using TMPro; // Necessary to control TextMeshProUGUI components
using UnityEngine;

public class LightController : MonoBehaviour
{
    // Public variables for Inspector connections (Task 3)
    
    [Tooltip("Renderer component of the LightBulb object (controls color)")]
    public Renderer lightBulbRenderer; 
    
    [Tooltip("TextMeshProUGUI object that displays the current status")]
    public TextMeshProUGUI statusText; 
    
    [Tooltip("The Info Panel GameObject to be activated/deactivated")]
    public GameObject infoPanel; 

    // Start function: Sets the initial state (Matches 'Initial State' criteria)
    void Start()
    {
        // 1. InfoPanel: hidden (Deactivate)
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
        
        // 2. StatusText: "Brightness: Dark"
        if (statusText != null)
        {
            statusText.text = "Brightness: Dark";
        }

        // 3. Set LightBulb color to Grey (matching the initial 'Dark' state)
        if (lightBulbRenderer != null)
        {
            lightBulbRenderer.material.color = Color.gray; 
        }
    }

    // ===============================================
    // SetDark() Function: Called on DarkButton click
    // ===============================================
    public void SetDark()
    {
        // LightBulb Color: Change to Grey
        if (lightBulbRenderer != null)
        {
            lightBulbRenderer.material.color = Color.gray;
        }

        // StatusText: "Brightness: Dark"
        if (statusText != null)
        {
            statusText.text = "Brightness: Dark";
        }

        // InfoPanel: Deactivate (Disappears)
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    // ===============================================
    // SetBright() Function: Called on BrightButton click
    // ===============================================
    public void SetBright()
    {
        // LightBulb Color: Change to White
        if (lightBulbRenderer != null)
        {
            lightBulbRenderer.material.color = Color.white;
        }
        
        // StatusText: "Brightness: Bright"
        if (statusText != null)
        {
            statusText.text = "Brightness: Bright";
        }
        
        // InfoPanel: Activate (Appears)
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
        }
    }
}