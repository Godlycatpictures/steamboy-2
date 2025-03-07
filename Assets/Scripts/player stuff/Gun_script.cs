using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootypewpew : MonoBehaviour
{
    public float mousex;
    public float mousey;
    public Transform Player;
    public Vector3 offset;
    public Camera SceneCamera;
    public Vector2 mousePosition;
    public Rigidbody2D rb;
    public Animator animator;

    void Update()
    {
        ProcessInputs();
        transform.position = Player.position + offset;
        animator.SetFloat("mousex", mousex);
    }

    void FixedUpdate()
    {
        mousePosition = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void ProcessInputs()
    {
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rb.rotation = aimAngle;

        mousex = mousePosition.x - rb.position.x;
    }
}