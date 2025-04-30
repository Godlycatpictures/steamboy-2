using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 0.5f;

    public Rigidbody2D rb;
    public Animator animator;
    public float movex;
    public float movey;
    public Weapon weapon;

    [Header("Dash Settings")]
    [SerializeField] float dashingspeed = 10;
    [SerializeField] float dashtimetaken = 1;
    [SerializeField] float dashcooldownbeforeotheruse = 1;

    private Vector2 directionofmovement;
    public float currently_dashing = 0;
    public bool can_dash = true;

    Vector2 movement;

    private NewBehaviourScript healthScript;

    // 🧠 These are the layer numbers
    private int playerLayer = 8;
    private int projectileLayer = 7;

    void Start()
    {
        healthScript = GetComponent<NewBehaviourScript>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movex = movement.x;
        movey = movement.y;

        animator.SetFloat("X_velocity", movement.x);
        animator.SetFloat("Y_velocity", movement.y);
        animator.SetFloat("Is_rolling", currently_dashing);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.Space) && can_dash)
        {
            StartCoroutine(Dash());
        }

        ProcessInputs();

        if (movement.x > 0f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (movement.x < 0f)
            transform.rotation = Quaternion.Euler(0, 180f, 0);
    }

    void FixedUpdate()
    {
        if (currently_dashing == 1)
            return;

        Move();

        void Move()
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void ProcessInputs()
    {
        directionofmovement = new Vector2(movement.x, movement.y).normalized;
    }

    private IEnumerator Dash()
    {
        currently_dashing = 1;
        can_dash = false;

        if (healthScript != null)
            healthScript.isInvincible = true;

        // 🚫 Ignore collisions with enemy projectiles during dash
        Physics2D.IgnoreLayerCollision(playerLayer, projectileLayer, true);

        rb.linearVelocity = directionofmovement * dashingspeed;

        yield return new WaitForSeconds(dashtimetaken);

        currently_dashing = 0;

        if (healthScript != null)
            healthScript.isInvincible = false;

        // ✅ Re-enable collision after dash
        Physics2D.IgnoreLayerCollision(playerLayer, projectileLayer, false);

        yield return new WaitForSeconds(dashcooldownbeforeotheruse);
        can_dash = true;
    }
}