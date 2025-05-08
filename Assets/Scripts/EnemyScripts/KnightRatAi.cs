using System.Collections;
using UnityEngine;

public class KnightRatAi : MonoBehaviour
{
    public float detectionRange = 6f;
    public float attackRange = 1.2f;
    public float speed = 1.5f;
    public float lungeSpeed = 5f;

    public float slashCooldown = 0f;
    public float lungeCooldown = 0f;

    public float xVelocity;
    public float lastKnownXVelocity = 1f;

    public bool isMoving;
    public bool isHurt;
    public bool slashing;
    public bool lunging;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    public GameObject slashPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (slashCooldown > 0) slashCooldown -= Time.deltaTime;
        if (lungeCooldown > 0) lungeCooldown -= Time.deltaTime;

        if (isHurt || slashing || lunging)
        {
            // Do nothing during attack or hurt
            return;
        }

        if (distanceToPlayer <= attackRange && slashCooldown <= 0f)
        {
            StartCoroutine(SlashAttack());
        }
        else if (distanceToPlayer <= detectionRange && lungeCooldown <= 0f)
        {
            StartCoroutine(LungeAttack());
        }
        else if (distanceToPlayer <= detectionRange)
        {
            MoveTowards(player.position);
            isMoving = true;
        }
        else
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
        }

        xVelocity = rb.velocity.x;
        if (xVelocity != 0)
            lastKnownXVelocity = xVelocity;
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private IEnumerator SlashAttack()
    {
        slashing = true;
        isMoving = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1.10f); // Wind-up

        if (slashPrefab != null)
        {
            Vector2 offset = new Vector2(Mathf.Sign(lastKnownXVelocity) * 0.6f, 0);
            Instantiate(slashPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.9f); // Recovery

        slashing = false;
        slashCooldown = 2f;
    }

    private IEnumerator LungeAttack()
    {
        isMoving = true;

        // Brief walk toward player to lock in direction
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
        yield return new WaitForSeconds(1.3f);
        // Begin lunge
        isMoving = false;
        rb.velocity = Vector2.zero;
        lunging = true;
        rb.velocity = direction * lungeSpeed;

        yield return new WaitForSeconds(0.5f); // Lunge movement time

        lunging = false;
        rb.velocity = Vector2.zero;
                
        yield return new WaitForSeconds(1f); // Lunge exiting time

        lungeCooldown = 5f;
    }

    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", lastKnownXVelocity);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isHurt", isHurt);
        animator.SetBool("slashing", slashing);
        animator.SetBool("lunging", lunging);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("DroneProjectile"))
        {
            isHurt = true;
            StartCoroutine(Hurt());
        }
    }

    private IEnumerator Hurt()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.8f);
        isHurt = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}