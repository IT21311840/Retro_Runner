using UnityEngine;

public class Finish : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject confetti;

    [Header("Manager Refs")]
    [SerializeField] private CheckpointManager checkpointManager;

    private bool levelCompleted = false;
    private static readonly int onCompleteAnim = Animator.StringToHash("OnComplete");

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (checkpointManager == null)
            checkpointManager = FindObjectOfType<CheckpointManager>();

        if (confetti != null)
            confetti.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !levelCompleted)
        {
            if (animator != null)
                animator.SetTrigger(onCompleteAnim);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(AudioType.levelFinish);
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null in Finish.");
            }

            if (confetti != null)
                confetti.SetActive(true);

            if (checkpointManager != null)
                checkpointManager.ClearCheckpoints();
            else
                Debug.LogWarning("CheckpointManager is null in Finish.");

            levelCompleted = true;
            Invoke(nameof(CompleteLevel), 2.15f);
        }
    }

    private void CompleteLevel()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.MarkLevelComplete();
            LevelManager.Instance.GoToNextLevel();
        }
        else
        {
            Debug.LogWarning("LevelManager.Instance is null in Finish.");
        }
    }
}
