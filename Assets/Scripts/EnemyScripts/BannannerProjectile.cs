using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannannerProjectile : MonoBehaviour
{
    private float speed = 3f;

    public GameObject impactEffect;

    private Vector2 aimPlayer;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    [SerializeField] private float trackingSmoothness = 2f; // How quickly the projectile updates its aim
    [SerializeField] private float maxTurnRate = 180f; // degrees per second

    private Vector2 currentDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        aimPlayer = player.position;
        currentDirection = (player.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        // Smoothly update the lagging target position
        aimPlayer = Vector2.Lerp(aimPlayer, player.position, trackingSmoothness * Time.fixedDeltaTime);

        // Desired direction toward lagged position
        Vector2 desiredDirection = (aimPlayer - (Vector2)transform.position).normalized;

        // Limit turning speed
        currentDirection = RotateTowards(currentDirection, desiredDirection, maxTurnRate * Mathf.Deg2Rad * Time.fixedDeltaTime);
        rb.velocity = currentDirection * speed;

        // Rotate sprite to face direction
        float aimAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle;
    }

    Vector2 RotateTowards(Vector2 from, Vector2 to, float maxRadiansDelta)
    {
        float angle = Vector2.SignedAngle(from, to);
        float angleToRotate = Mathf.Clamp(angle, -maxRadiansDelta * Mathf.Rad2Deg, maxRadiansDelta * Mathf.Rad2Deg);
        float finalAngle = Mathf.Atan2(from.y, from.x) + angleToRotate * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle)).normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
