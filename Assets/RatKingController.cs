using System.Collections;
using UnityEngine;

public class RatKingBoss : MonoBehaviour
{
    public Transform scepterHand;
    public Transform orbHand;
    public Animator ratAnimator;

    public float moveCooldown = 3f;
    public float dashCooldown = 5f;
    public float moveSpeed = 1.5f;
    public float dashForce = 10f;

    private float moveTimer;
    private float dashTimer;
    private Rigidbody2D rb;
    private Transform player;

    private Vector3 scepterOffset = new Vector3(-1.2f, 0f, 0f);
    private Vector3 orbOffset = new Vector3(1.2f, 0f, 0f);

    private bool isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;

        if (!isDashing)
        {
            if (dashTimer <= 0f)
            {
                StartCoroutine(DashAttack());
            }
            else if (moveTimer <= 0f)
            {
                StartCoroutine(WalkAround());
                moveTimer = moveCooldown;
            }
        }

        // Always keep hands hovering relative to body
        scepterHand.position = transform.position + scepterOffset;
        orbHand.position = transform.position + orbOffset;
    }

    private IEnumerator WalkAround()
    {
        ratAnimator.SetBool("isMoving", true);
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        float moveTime = Random.Range(0.7f, 1.5f);

        float timer = 0f;
        while (timer < moveTime)
        {
            rb.velocity = dir * moveSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        ratAnimator.SetBool("isMoving", false);
    }

    private IEnumerator DashAttack()
    {
        isDashing = true;
        ratAnimator.SetBool("attacking", true);

        yield return new WaitForSeconds(0.6f); // Delay before dash, sync with windup anim

        Vector2 dashDir = ((Vector2)player.position - rb.position).normalized;
        rb.velocity = dashDir * dashForce;

        yield return new WaitForSeconds(0.3f); // Dash duration
        rb.velocity = Vector2.zero;

        ratAnimator.SetBool("attacking", false);
        isDashing = false;
        dashTimer = dashCooldown;
    }
}