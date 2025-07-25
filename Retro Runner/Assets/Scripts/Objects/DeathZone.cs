using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        var life = collision.GetComponent<PlayerLife>();
        if (life != null)
            life.Die();
        else
            Debug.LogWarning("PlayerLife component not found on Player in DeathZone.");
    }
}
