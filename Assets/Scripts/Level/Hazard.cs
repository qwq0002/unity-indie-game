using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null && !playerRespawn.IsInvulnerable())
            {
                playerRespawn.Respawn();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerRespawn playerRespawn = collision.gameObject.GetComponent<PlayerRespawn>();
            if (playerRespawn != null && !playerRespawn.IsInvulnerable())
            {
                playerRespawn.Respawn();
            }
        }
    }
}
