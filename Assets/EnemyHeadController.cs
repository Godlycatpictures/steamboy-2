using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float dashSpeed = 6f;
    public float dashCooldown = 5f;
    public float dashDuration = 0.5f;
    public Animator animator;

    private Transform player;
    private Rigidbody2D rb;
    private bool isDashing;
    private float lastDashTime = Mathf.NegativeInfinity;

    public GameObject lungePrefab;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDashing) return;

        if (Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
            return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;
        animator.SetBool("isMoving", true);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        animator.SetBool("isMoving", false);
        animator.SetBool("attacking", true);

        rb.velocity = (player.position - transform.position).normalized * dashSpeed;

        GameObject lungeAttack = Instantiate(lungePrefab, transform.position, Quaternion.identity);
        KingRatLungeAttack lungeScript = lungeAttack.GetComponent<KingRatLungeAttack>();
        lungeScript.ratKing = transform;
        lungeScript.EnemyHeadController = this;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        animator.SetBool("attacking", false);
        isDashing = false;

        lastDashTime = Time.time; // ← move cooldown reset here!
    }
}