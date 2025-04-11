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
    public float lastKnownXVelocity = 1f; // Default to facing right
    public float attackCoolDown = 0f; // Starts at 0, ready to attack

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

        // Reduce cooldown timer
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }

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

    private void ChooseAttack()
    {

        StartCoroutine(FireOrbAttack());
        
        StartCoroutine(SpikeAttack());

        attackCoolDown = 1.5f; // Reset cooldown
    }

    private IEnumerator FireOrbAttack()
    {

        yield return new WaitForSeconds(1f); // Simulated attack duration

    }

    private IEnumerator SpikeAttack()
    {
        
        yield return new WaitForSeconds(1f); // Simulated attack duration

    }


    private void FixedUpdate()
    {
        // Use last known xVelocity to ensure direction remains consistent
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
