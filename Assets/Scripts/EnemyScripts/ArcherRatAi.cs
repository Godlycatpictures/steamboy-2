using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherRatAi : MonoBehaviour
{
    public float detectionRange = 7f;
    public float attackRange = 4f;
    public float retreatRange = 2f;
    public float speed = 1.5f;
    public float attackCoolDown = 0f;

    public float xVelocity;
    public float lastKnownXVelocity = 1f;

    public bool isMoving;
    public bool attacking;
    public bool isHit;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    public GameObject arrowPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (attackCoolDown > 0)
            attackCoolDown -= Time.deltaTime;

        if (distanceToPlayer < detectionRange && !attacking && !isHit)
        {
            if (distanceToPlayer < retreatRange)
            {
                // Player is too close — retreat!
                RetreatFromPlayer();
            }
            else if (distanceToPlayer > attackRange)
            {
                // Player is in range but not too close — move towards
                isMoving = true;
                MoveTowards(player.position);
            }
            else if (attackCoolDown <= 0)
            {
                // Player is in attack range — shoot
                isMoving = false;
                StartCoroutine(Attack());
            }
            else if (attackCoolDown <= 0)
            {
                isMoving = false;
                FacePlayer();
                StartCoroutine(Attack());
            }
            }
            else
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
            }

        xVelocity = rb.velocity.x;
        if (xVelocity != 0)
            lastKnownXVelocity = xVelocity;
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
    }

    void RetreatFromPlayer()
    {
        Vector2 awayFromPlayer = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.velocity = awayFromPlayer * speed;
        isMoving = true;
    }

IEnumerator Attack()
{
    rb.velocity = Vector2.zero;
    attacking = true;
    isMoving = false;

    FacePlayer(); // Make sure we face the player before attacking

    yield return new WaitForSeconds(2.5f); // Delay to sync with animation if needed

    if (arrowPrefab != null)
    {
        Vector2 shootDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        if (arrowRb != null)
        {
            float arrowSpeed = 7f; // Adjust as needed for gameplay
            arrowRb.velocity = shootDirection * arrowSpeed;

            // Rotate arrow to match flight direction
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    yield return new WaitForSeconds(0.5f); // Short post-attack delay

    attackCoolDown = 2.5f;
    attacking = false;
}

    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", lastKnownXVelocity);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("attacking", attacking);
        animator.SetBool("isHit", isHit);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("DroneProjectile"))
        {
            isHit = true;
            StartCoroutine(Hurt());
        }
    }
    void FacePlayer()
    {
        if (player != null)
        {
            float directionToPlayer = player.position.x - transform.position.x;
            lastKnownXVelocity = Mathf.Sign(directionToPlayer);
        }
    }

    IEnumerator Hurt()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        isHit = false;
        isMoving = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, retreatRange);
    }
}