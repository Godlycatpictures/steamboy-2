using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Transform player; // Referens till spelaren
    public Vector3 offset = new Vector3(-1, -1, -1); // Sköldens position bakom spelaren

    void Update()
    {
        if (player != null)
        {
            // Håller skölden bakom spelaren
            transform.position = player.position + player.rotation * offset;

            // Roterar skölden med spelaren
            transform.rotation = player.rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyProjectile")) // Om en fiendeprojektil träffar skölden
        {
            Destroy(collision.gameObject); // Förstör projektilen
        }
    }
}


