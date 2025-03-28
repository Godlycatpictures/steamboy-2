using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public float lifetime = 10f; // Added death timer (10 seconds)
    public GameObject explosion;
    
    private Transform target;
    private Rigidbody2D rb;

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
        
        // Add automatic destruction after lifetime seconds
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            target = FindClosestEnemy();
            if (target == null) return;
        }

        Vector2 direction = (Vector2)target.position - rb.position;
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
    
void OnCollisionEnter2D(Collision2D collision)
{
    Explode();
}

void Explode()
{
    if (explosion != null)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
    
    Destroy(gameObject); // Destroy missile immediately after spawning explosion
}
}