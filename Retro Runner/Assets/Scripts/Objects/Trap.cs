using UnityEngine;

public class Trap : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLife life = collision.gameObject.GetComponent<PlayerLife>();
            if (life != null)
                life.Die();
            else
                Debug.LogWarning("PlayerLife component missing on Player.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerLife life = collision.gameObject.GetComponent<PlayerLife>();
            if (life != null)
                life.Die();
            else
                Debug.LogWarning("PlayerLife component missing on Player.");
        }
    }
}
