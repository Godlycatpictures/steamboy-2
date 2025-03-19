using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimationController : MonoBehaviour
{
    public float xVelocity;
    public float lastKnownXVelocity = 1f; // Default to facing right
    private Animator animator;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xVelocity = rb.velocity.x;

        if (xVelocity != 0)
        {
            lastKnownXVelocity = xVelocity;
        }
        
    }

    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", lastKnownXVelocity); 
    }
}
