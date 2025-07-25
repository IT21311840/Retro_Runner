using TMPro;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D boxCollider;
    
    [Header("Life counts")]
    [SerializeField] private int LifeCount = 3;
    [SerializeField] private int maxLifeBound = 100;
    [SerializeField] private TextMeshProUGUI heartsCount;
    // [SerializeField] private GameObject[] hearts;
    
    [Header("Respawn Fields")]
    [SerializeField] private Vector3 initSpawnPos;
    [SerializeField] private Vector3 respawnOffset;
    
    [Header("Manager Refs")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameOverMenu gameOverMenu;
    [SerializeField] private CheckpointManager checkpointManager;

    [SerializeField] private CharacterDataSO charData;
    
    private static readonly int deathAnim = Animator.StringToHash("death");

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        if (gameOverMenu == null) gameOverMenu = FindObjectOfType<GameOverMenu>();
        if (checkpointManager == null) checkpointManager = FindObjectOfType<CheckpointManager>();

        int initLifeCnt = charData.playerLifeData.InitLifeCount;
        LifeCount = PlayerPrefs.GetInt("LifeCount", initLifeCnt);
        PlayerPrefs.SetInt("LifeCount", LifeCount);
        maxLifeBound = charData.playerLifeData.MaxLivesBound;
        
        UpdateHeartsCountsUI();
    }

    public void Die()
    {
        if (boxCollider != null) 
            boxCollider.enabled = false;

        if (CameraShaker.Instance != null)
            CameraShaker.Instance.ShakeCamera(5f, 0.25f);
        else
            Debug.LogWarning("CameraShaker.Instance is null in PlayerLife.Die");

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySound(AudioType.characterDeath);
        else
            Debug.LogWarning("AudioManager.Instance is null in PlayerLife.Die");

        if (animator != null)
            animator.SetTrigger(deathAnim);

        LoseLife();
        Invoke(nameof(CheckLifeStatus), 1.5f);
    }

    private void LoseLife()
    {
        if (LifeCount > 0)
            LifeCount--;

        PlayerPrefs.SetInt("LifeCount", LifeCount);
        UpdateHeartsCountsUI();
    }

    public void GiveLife(int count = 1)
    {
        if (LifeCount < maxLifeBound)
            LifeCount += count;

        PlayerPrefs.SetInt("LifeCount", LifeCount);
        UpdateHeartsCountsUI();
    }

    private void UpdateHeartsCountsUI()
    {
        if (heartsCount != null)
            heartsCount.text = $"{LifeCount}";
        else
            Debug.LogWarning("heartsCount TextMeshProUGUI not assigned in PlayerLife.");
    }
    
    private void CheckLifeStatus()
    {
        // DecreaseHearts();
        UpdateHeartsCountsUI();
        
        if (LifeCount > 0)
        {
            Respawn();
        }
        else
        {
            if (gameOverMenu != null)
                gameOverMenu.OpenGameOverMenu();
            else
                Debug.LogWarning("GameOverMenu not assigned in PlayerLife.");
        }
    }

    private void Respawn()
    {
        if (boxCollider != null) 
            boxCollider.enabled = true;
        if (playerMovement != null) 
            playerMovement.Flip(true);
        
        if (checkpointManager != null && checkpointManager.hasPassedAnyCheckPoints())
        {
            Vector3 pos = checkpointManager.GetLatestCheckPoint().position;
            transform.position = new Vector3(pos.x + respawnOffset.x, pos.y + respawnOffset.y, pos.z + respawnOffset.z);
        }
        else
        {
            transform.position = initSpawnPos;
        }
    }
}
