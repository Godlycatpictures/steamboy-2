using System.Collections;
using UnityEngine;

public class KnightRatAi : MonoBehaviour
{
    public float detectionRange = 6f;
    public float attackRange = 1.2f;
    public float speed = 1.5f;
    public float lungeSpeed = 5f;

    public int facingDirection;

    public float slashCooldown = 1f;
    public float lungeCooldown = 1f;

    public float xVelocity;
    public float lastKnownXVelocity = 1f;

    public bool isMoving;
    public bool isHurt;
    public bool slashing;
    public bool lunging;
    public bool canLunge = true;
    public bool inRange;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    public GameObject slashPrefab;
    public GameObject lungePrefab;


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

        // Don’t interrupt attacks by forcibly zeroing velocity here
        if (isHurt || slashing || lunging)
        {
            return;
        }

        if (distanceToPlayer <= attackRange && slashCooldown <= 0f && !lunging && !slashing)
        {
            StartCoroutine(SlashAttack());
        }
        else if (distanceToPlayer <= detectionRange && lungeCooldown <= 0f && !lunging && !slashing && canLunge)
        {
            StartCoroutine(LungeAttack());
        }
        else if (distanceToPlayer <= detectionRange && !inRange)
        {
            MoveTowards(player.position);
            isMoving = true;
        }
        else
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
        }
        if(distanceToPlayer <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        xVelocity = rb.velocity.x;
        if (xVelocity != 0)
            lastKnownXVelocity = xVelocity;
        
        if (lastKnownXVelocity >0)
        {
            facingDirection = 1;
        }
        else if (lastKnownXVelocity < 0)
        {
            facingDirection = -1;
        }
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

        yield return new WaitForSeconds(1.1f); // Wind-up

        if (slashPrefab != null)
        {
            Vector2 offset = new Vector2(Mathf.Sign(lastKnownXVelocity) * 1.2f, 0);
            Instantiate(slashPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.5f); // Recovery

        slashing = false;
        slashCooldown = 3f;
    }

    private IEnumerator LungeAttack()
    {
        isMoving = true;
        canLunge = false;

        // Lock in direction at the start
        Vector2 lockedDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;

        // Brief approach
        rb.velocity = lockedDirection * speed;
        yield return new WaitForSeconds(1f);

        // Wind-up
        rb.velocity = Vector2.zero;
        isMoving = false;
        lunging = true;

        yield return new WaitForSeconds(1.3f);

        // Lunge!
        rb.velocity = lockedDirection * lungeSpeed;

        GameObject lungeAttack = Instantiate(lungePrefab, transform.position, Quaternion.identity);
        KnightRatLungeAttack lungeScript = lungeAttack.GetComponent<KnightRatLungeAttack>();
        lungeScript.ratKnight = transform;
        lungeScript.knightRatAi = this;


        yield return new WaitForSeconds(0.5f);

        // End lunge
        rb.velocity = Vector2.zero;
        lunging = false;

        yield return new WaitForSeconds(1f);

        canLunge = true;
        lungeCooldown = 6f;
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