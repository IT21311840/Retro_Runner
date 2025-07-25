using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D checkpointCollider;

    private static readonly int onCompleteAnim = Animator.StringToHash("OnComplete");

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (checkpointCollider == null)
            checkpointCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var checkpointManager = GetComponentInParent<CheckpointManager>();
            if (checkpointManager != null)
            {
                checkpointManager.SetLatestCheckpoint(transform);
            }
            else
            {
                Debug.LogWarning("CheckpointManager not found on parent.");
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(AudioType.checkpointPass);
            }
            else
            {
                Debug.LogWarning("AudioManager.Instance is null in Checkpoint.");
            }

            if (animator != null)
                animator.SetTrigger(onCompleteAnim);

            if (checkpointCollider != null)
                checkpointCollider.enabled = false;
        }
    }
}
