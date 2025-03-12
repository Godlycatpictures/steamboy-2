using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannannerProjectile : MonoBehaviour
{
    private float speed = 1f;

    public GameObject impactEffect;

    private Vector2 aimPlayer;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {

        MoveTowards(player.position);

        aimPlayer = player.position;
        Vector2 aimDirection = aimPlayer - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle;

    }
    
    private void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed; // Moves in both X and Y directions
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Instantiate(impactEffect, transform.position, Quaternion.identity); //Impact effect
        Destroy(gameObject);
    }
}
