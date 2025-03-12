using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinAi : MonoBehaviour
{
    private float detectionRange = 3f;
    private float attackRange = 1f;
    private float speed = 0.5f;
    public float xVelocity;
    public float yVelocity;
    public float lastKnownXVelocity = 1f; // Default to facing right
    public float attackCoolDown = 0f; // Starts at 0, ready to attack

    public bool isMoving;
    public bool attacking;

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
            else if (distanceToPlayer <= attackRange && attackCoolDown <= 0)
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

        // Attack logic (e.g., play animation, deal damage, etc.)

        yield return new WaitForSeconds(1f); // Simulated attack duration

        //Instantiate(attackPrefab, rb.position, Quaternion.identity); Lägg till attack här

        yield return new WaitForSeconds(0.5f); // Added time for padding

        attacking = false;
        attackCoolDown = 1.5f; // Reset cooldown
    }

    private void FixedUpdate()
    {
        // Use last known xVelocity to ensure direction remains consistent
        animator.SetFloat("xVelocity", lastKnownXVelocity); 
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("attacking", attacking);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
