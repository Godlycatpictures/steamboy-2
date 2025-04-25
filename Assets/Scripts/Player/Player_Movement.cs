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

    //dashingeg
    [Header("Setting for the dash")] // g�r ddet enklare att se valuen i editor
    [SerializeField] float dashingspeed = 10;
    [SerializeField] float dashtimetaken = 1;
    [SerializeField] float dashcooldownbeforeotheruse = 1;
    private Vector2 directionofmovement;
    public float currently_dashing = 0;
    public bool can_dash = true;

    Vector2 movement;

    void Update()
    {

        //Kopplar hastigheterna i olika riktningar till animationerna.
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movex = movement.x;
        movey = movement.y;

        animator.SetFloat("X_velocity", movement.x);
        animator.SetFloat("Y_velocity", movement.y);
        animator.SetFloat("Is_rolling", currently_dashing);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // dash keybind
        if (Input.GetKeyDown(KeyCode.Space) && can_dash)
        {

            StartCoroutine(Dash());
        }

        ProcessInputs();

        //Vinkar karakt�ren �t andra h�let om x movement �r negativ
        if (movement.x > 0f)
        {

            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else if (movement.x < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
    }

    void FixedUpdate()
    {
        if (currently_dashing == 1)
        {
            return;  // ifall dash --> inte g�r n�t annat
        }

        Move();

        void Move()
        {
            //walking script
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        }
    }

    void ProcessInputs()
    {
        directionofmovement = new Vector2(movement.x, movement.y).normalized;
    }

    private IEnumerator Dash() // dash
    {

        currently_dashing = 1;

        can_dash = false;
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        rb.velocity = new Vector2(directionofmovement.x * dashingspeed, directionofmovement.y * dashingspeed);

        yield return new WaitForSeconds(dashtimetaken); // s�tter tiden det tar att dashar

        if (col != null)
            col.enabled = true;

        currently_dashing = 0;

        yield return new WaitForSeconds(dashcooldownbeforeotheruse);
        can_dash = true;
    }
}