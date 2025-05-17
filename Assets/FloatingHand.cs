using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingHand : MonoBehaviour
{
    public Transform head; // assign the head transform
    public Vector2 offset; // position relative to the head
    private float stiffness = 100f; // how strongly it moves to offset
    private float damping = 50; // how much it resists fast movement

    private Vector2 velocity;

    void Update()
    {
        Vector2 targetPos = (Vector2)head.position + offset;
        Vector2 currentPos = transform.position;
        Vector2 force = (targetPos - currentPos) * stiffness - velocity * damping;

        velocity += force * Time.deltaTime;
        currentPos += velocity * Time.deltaTime;

        transform.position = currentPos;
    }
}