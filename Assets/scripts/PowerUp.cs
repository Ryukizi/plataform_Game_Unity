using UnityEngine;

public class PowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player get powerUp");
            Destroy(gameObject);
        }
    }
}
