using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinAi : MonoBehaviour
{
    private float detectionRange = 6f;
    private float attackRange = 3f;
    private float speed = 0.5f;
    public float xVelocity;
    public float yVelocity;
    public float lastKnownXVelocity = 1f; // Default to facing right
    public float attackCoolDown = 0f; // Starts at 0, ready to attack

    public bool isMoving;
    public bool attacking;
    public bool isHit;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    public GameObject attackPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Reduce cooldown timer
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }

        if (distanceToPlayer < detectionRange)
        {
            if (distanceToPlayer > attackRange && !attacking)
            {
                isMoving = true;
                MoveTowards(player.position);
            }
            else if (distanceToPlayer <= attackRange && attackCoolDown <= 0 && !attacking)
            {
                isMoving = false;
                StartCoroutine(Attack());
            }
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector2.zero; // Stop movement when player is out of range
        }

        // Update velocity values for the animator
        xVelocity = rb.velocity.x;

        // Store last known xVelocity if moving
        if (xVelocity != 0)
        {
            lastKnownXVelocity = xVelocity;
        }
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed; // Moves in both X and Y directions
    }

private IEnumerator Attack()
{
    rb.velocity = Vector2.zero; // Stop movement while attacking
    attacking = true;

    yield return new WaitForSeconds(0.8f); // Simulated attack duration

    int projectileCount = 7; // Number of projectiles
    float projectileSpeed = 3f; // Adjust as needed

    for (int i = 0; i < projectileCount; i++)
    {
        // Generate a random angle between 10° and 170° (Only above the enemy)
        float randomAngle = Random.Range(10f, 170f);
        float radianAngle = randomAngle * Mathf.Deg2Rad; // Convert to radians

        // Convert angle to direction vector
        Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle)).normalized;

        // Calculate the rotation to match the movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convert to degrees
        Quaternion rotation = Quaternion.Euler(0, 0, angle); // 2D rotation

        // Instantiate projectile with the correct rotation
        var UrchinProjectile = Instantiate(attackPrefab, rb.position, rotation);
        Rigidbody2D projectileRb = UrchinProjectile.GetComponent<Rigidbody2D>();

        if (projectileRb != null)
        {
            // Assign velocity
            projectileRb.velocity = direction * projectileSpeed;
        }
    }

    yield return new WaitForSeconds(0.8f); // Added time for padding

    attacking = false;
    attackCoolDown = 1.5f; // Reset cooldown
}

    private void FixedUpdate()
    {
        // Use last known xVelocity to ensure direction remains consistent
        animator.SetFloat("xVelocity", lastKnownXVelocity); 
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("attacking", attacking);
        animator.SetBool("isHit", isHit);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.CompareTag("Bullet")) // Check if it's a bullet
    { 

        isHit = true;
        StartCoroutine(Hurt());

    } if (collision.CompareTag("DroneProjectile")) //chech if le drone projectile
    {

    }
    }

    private IEnumerator Hurt()
    {
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(1f);

            isHit = false;
            isMoving = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
