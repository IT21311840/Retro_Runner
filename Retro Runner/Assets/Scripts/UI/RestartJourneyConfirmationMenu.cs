using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartJourneyConfirmationMenu : MonoBehaviour
{
    [SerializeField] private Canvas confirmationCanvas;

    public void OpenConfirmationPanel()
    {
        if (confirmationCanvas != null)
            confirmationCanvas.enabled = true;
        else
            Debug.LogWarning("confirmationCanvas not assigned in RestartJourneyConfirmationMenu.");
    }

    public void OnConfirm()
    {
        // Safely reset level progress
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ResetLevelProgress();
        }
        else
        {
            Debug.LogWarning("LevelManager.Instance is null in RestartJourneyConfirmationMenu.OnConfirm.");
        }

        // Load the start screen (make sure this name matches your build settings)
        SceneManager.LoadScene("Start Screen");

        // Reset player prefs
        PlayerPrefs.SetInt("Character", 0);
        PlayerPrefs.SetInt("LifeCount", 3);
        PlayerPrefs.SetInt("Foods", 0);

        // Resume time
        Time.timeScale = 1;
    }

    public void OnReject()
    {
        if (confirmationCanvas != null)
            confirmationCanvas.enabled = false;
        else
            Debug.LogWarning("confirmationCanvas not assigned in RestartJourneyConfirmationMenu.");
    }
}
