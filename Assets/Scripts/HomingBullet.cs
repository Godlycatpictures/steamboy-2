using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float lifetime = 5f;

    private Transform target;
    private Rigidbody2D rb;
    private GameObject targetGuide; // Osynligt objekt som leder missilen

    public void Initialize(float bulletSpeed, float bulletRotateSpeed)
    {
        speed = bulletSpeed;
        rotateSpeed = bulletRotateSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = FindClosestEnemy();
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        if (target != null)
        {
            // Skapa ett osynligt objekt framför missilen som leder den
            targetGuide = new GameObject("TargetGuide");
            targetGuide.transform.position = transform.position;
        }

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            target = FindClosestEnemy();
            if (target == null) return;
        }

        // Flytta targetGuide snabbare än missilen för att skapa en jämnare bana
        if (targetGuide != null)
        {
            targetGuide.transform.position = Vector2.MoveTowards(
                targetGuide.transform.position,
                target.position,
                speed * 1.5f * Time.fixedDeltaTime // Guide rör sig snabbare än missilen
            );
        }

        // Rikta missilen mot targetGuide istället för direkt mot fienden
        Vector2 direction = (Vector2)targetGuide.transform.position - rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float currentDistance = Vector2.Distance(transform.position, enemy.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closest = enemy.transform;
            }
        }
        return closest;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Förstör missilen
            if (targetGuide != null)
            {
                Destroy(targetGuide); // Förstör även targetGuide
            }
        }
    }
}