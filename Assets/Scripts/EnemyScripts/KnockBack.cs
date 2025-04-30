using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private float knockbackForce = 5f; // Adjust this to control how strong the knockback is
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Ensure the enemy has a Rigidbody2D
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("DroneProjectile"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            ApplyKnockback(knockbackDirection);
        }
    }

    private void ApplyKnockback(Vector2 direction)
    {
        rb.linearVelocity = Vector2.zero; // Reset velocity for consistent knockback
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}
