using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRatAi : MonoBehaviour
{
    private float detectionRange = 10f;
    private float attackRange = 2f;
    private float speed = 1f;
    public float xVelocity;
    public float yVelocity;
    public float lastKnownXVelocity = 1f;

    public float attackCoolDown = 0f;

    public bool isMoving;
    public bool exploding;
    public bool isHit;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    public GameObject attackPrefab;
    public GameObject deathEffect;

    [Header("Gore Settings")]
    public GameObject[] gorePrefabs;
    public GameObject[] bloodSplats;
    public int goreAmount = 5;
    public float goreSpreadForce = 5f;
    public float goreLifetime = 2f;
    public int bloodAmount = 3;

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

        if (distanceToPlayer < detectionRange)
        {
            if (distanceToPlayer > attackRange && !exploding && !isHit)
            {
                isMoving = true;
                MoveTowards(player.position);
            }
            else if (distanceToPlayer <= attackRange && attackCoolDown <= 0 && !exploding)
            {
                isMoving = false;
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

    private void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private IEnumerator Attack()
    {
        rb.velocity = Vector2.zero;
        exploding = true;

        yield return new WaitForSeconds(1.2f);

        Instantiate(attackPrefab, rb.position, Quaternion.identity);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        
        SpawnGore();
        SpawnBlood();

        Destroy(gameObject);
        attackCoolDown = 1.5f;
    }

    void SpawnGore()
    {
        if (gorePrefabs == null || gorePrefabs.Length == 0)
            return;

        for (int i = 0; i < goreAmount; i++)
        {
            int randomIndex = Random.Range(0, gorePrefabs.Length);
            Quaternion randomRot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            GameObject gorePiece = Instantiate(gorePrefabs[randomIndex], transform.position, randomRot);

            Rigidbody2D rbGore = gorePiece.GetComponent<Rigidbody2D>();
            if (rbGore != null)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rbGore.AddForce(randomDir * Random.Range(goreSpreadForce * 0.5f, goreSpreadForce), ForceMode2D.Impulse);
                rbGore.drag = Random.Range(3f, 7f);
                rbGore.angularDrag = Random.Range(3f, 7f);
            }

            if (Random.value < 0.5f)
                StartCoroutine(FadeAndDestroy(gorePiece, goreLifetime));
            else
                StartCoroutine(StickyGore(gorePiece));
        }
    }

    void SpawnBlood()
    {
        if (bloodSplats == null || bloodSplats.Length == 0)
            return;

        for (int i = 0; i < bloodAmount; i++)
        {
            int randomIndex = Random.Range(0, bloodSplats.Length);
            GameObject blood = Instantiate(bloodSplats[randomIndex]);

            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            blood.transform.position = (Vector2)transform.position + randomOffset;
            blood.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            float randomScale = Random.Range(0.8f, 1.2f);
            blood.transform.localScale = new Vector3(randomScale, randomScale, 1f);
        }
    }

    IEnumerator FadeAndDestroy(GameObject gorePiece, float lifetime)
    {
        SpriteRenderer sr = gorePiece.GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        Color originalColor = sr.color;

        yield return new WaitForSeconds(lifetime);
        float fadeDuration = 1f;

        while (elapsed < fadeDuration)
        {
            if (sr != null)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gorePiece);
    }

    IEnumerator StickyGore(GameObject gorePiece)
    {
        Rigidbody2D rb = gorePiece.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(0.5f);

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", lastKnownXVelocity);
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("exploding", exploding);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("DroneProjectile"))
        {
            isHit = true;
            StartCoroutine(Hurt());
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