using TMPro;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;
    [SerializeField] private TextMeshProUGUI cherriesText;

    void Start()
    {
        cherries = PlayerPrefs.GetInt("Foods", cherries);
        UpdateCherries();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(AudioType.itemCollect);
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null in ItemCollector.");
            }

            Destroy(collision.gameObject);
            cherries++;
            PlayerPrefs.SetInt("Foods", cherries);
            UpdateCherries();
        }
    }

    private void UpdateCherries()
    {
        if (cherriesText != null)
            cherriesText.text = $"{cherries}";
        else
            Debug.LogWarning("Cherries TextMeshProUGUI not assigned.");
    }
}
