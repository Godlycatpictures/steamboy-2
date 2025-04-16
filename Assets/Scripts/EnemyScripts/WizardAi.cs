using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAi : MonoBehaviour
{
    private float detectionRange = 8f;
    private float attackRange = 6f;
    private float speed = 0.5f;
    public float xVelocity;
    public float yVelocity;
    public float lastKnownXVelocity = 1f;

    public float attackCoolDown = 0f;

    public bool attacking;
    public bool spikeAttack;
    public bool fireOrbAttack;
    public bool hatMode;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    public GameObject spikePrefab;
    public GameObject fireOrbPrefab;

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
            if (distanceToPlayer > attackRange && !attacking)
            {
                MoveTowards(player.position);
            }
            else if (distanceToPlayer <= attackRange && attackCoolDown <= 0 && !attacking)
            {
                ChooseAttack();
            }
        }
        else
        {
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

    private void ChooseAttack()
    {
        attacking = true;

        float roll = Random.Range(0f, 1f);

        if (roll < 0.5f)
        {
            StartCoroutine(FireOrbAttack());
        }
        else if (roll < 0.8f)
        {
            StartCoroutine(HatDashAttack());
        }
        else
        {
            StartCoroutine(SpikeAttack());
        }
    }

private IEnumerator FireOrbAttack()
{
    rb.velocity = Vector2.zero;

    fireOrbAttack = true;

    yield return new WaitForSeconds(1.2f); // Wind-up

    int orbType = Random.Range(0, 3); // 0 = burst, 1 = single, 2 = spinning

    if (orbType == 0)
    {
        // BURST SHOT
        int orbCount = 10;
        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * Mathf.PI * 2f / orbCount;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject orb = Instantiate(fireOrbPrefab, transform.position, Quaternion.Euler(0, 0, zRotation));
            Rigidbody2D orbRb = orb.GetComponent<Rigidbody2D>();
            if (orbRb != null)
                orbRb.velocity = direction * 3f;
        }
    }
    else if (orbType == 1)
    {
        // SINGLE TARGETED SHOT
        Vector2 direction = (player.position - transform.position).normalized;
        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject orb = Instantiate(fireOrbPrefab, transform.position, Quaternion.Euler(0, 0, zRotation));
        Rigidbody2D orbRb = orb.GetComponent<Rigidbody2D>();
        if (orbRb != null)
            orbRb.velocity = direction * 5f;
    }
    else if (orbType == 2)
    {
        // SPINNING FIRE BURST
        int shots = 20;
        float startAngle = Random.Range(0f, 360f); // adds a little randomness
        for (int i = 0; i < shots; i++)
        {
            float angle = startAngle + (i * (360f / shots));
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            GameObject orb = Instantiate(fireOrbPrefab, transform.position, Quaternion.Euler(0, 0, zRotation));
            Rigidbody2D orbRb = orb.GetComponent<Rigidbody2D>();
            if (orbRb != null)
                orbRb.velocity = direction * 3f;

            yield return new WaitForSeconds(0.1f); // delay between each shot
        }
    }

    yield return new WaitForSeconds(0.2f);

    fireOrbAttack = false;
    attacking = false;
    attackCoolDown += 1.5f;
}


    private IEnumerator SpikeAttack()
    {
        rb.velocity = Vector2.zero;

        spikeAttack = true;

        yield return new WaitForSeconds(0.4f); // Wind-up

        int spikeCount = 20;
        float minRadius = 0f;
        float maxRadius = 3f;

        for (int i = 0; i < spikeCount; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(minRadius, maxRadius);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            Instantiate(spikePrefab, player.position + (Vector3)offset, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);

        spikeAttack = false;
        attacking = false;
        attackCoolDown += 1.5f;
    }

    private IEnumerator HatDashAttack()
    {
        hatMode = true;

        yield return new WaitForSeconds(1f); // Dash wind-up

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Vector2 dashDirection = (player.position - transform.position).normalized;
        rb.velocity = dashDirection * 5f;

        yield return new WaitForSeconds(1f); // Dash duration

        rb.velocity = Vector2.zero;

        if (col != null)
            col.enabled = true;

        hatMode = false;
        attacking = false;
        attackCoolDown += 1.5f;

        yield return new WaitForSeconds(1f); // Post-dash delay
    }

    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", lastKnownXVelocity);
        animator.SetBool("spikeAttack", spikeAttack);
        animator.SetBool("fireOrbAttack", fireOrbAttack);
        animator.SetBool("hatMode", hatMode);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
