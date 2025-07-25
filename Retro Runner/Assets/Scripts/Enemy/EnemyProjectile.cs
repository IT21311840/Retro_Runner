using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D coll;

    [Header("Movement")] 
    [SerializeField] private MovementDirection projectileDir = MovementDirection.left;
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;

    private float lifetime;
    private bool hit;

    private static readonly int explodeAnim = Animator.StringToHash("Explode");

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        if (coll == null)
            coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        if (coll != null)
            coll.enabled = true;
        else
            Debug.LogWarning("Collider not assigned on EnemyProjectile.");
    }

    private void Update()
    {
        if (hit) return;

        Move();

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void Move()
    {
        float movementSpeed = speed * Time.deltaTime;

        switch (projectileDir)
        {
            case MovementDirection.left:
            case MovementDirection.right:
            case MovementDirection.none:
                transform.Translate(movementSpeed, 0, 0);
                break;
            case MovementDirection.up:
            case MovementDirection.down:
                transform.Translate(0, movementSpeed, 0);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;

        if (collision.CompareTag("Player"))
        {
            PlayerLife playerLife = collision.GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                playerLife.Die();
            }
            else
            {
                Debug.LogWarning("PlayerLife component not found on Player in EnemyProjectile.");
            }
        }

        if (coll != null)
            coll.enabled = false;

        if (animator != null)
            animator.SetTrigger(explodeAnim);
        else
            Debug.LogWarning("Animator not assigned on EnemyProjectile.");
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
