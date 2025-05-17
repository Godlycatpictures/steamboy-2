using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinProjectile : MonoBehaviour
{
 public float speed = 10f;
    public float life = 3f;
    public GameObject impactEffect;

    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Optional: damage logic
        if (collision.gameObject.CompareTag("Player"))
        {
            // collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(1);
        }

        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
