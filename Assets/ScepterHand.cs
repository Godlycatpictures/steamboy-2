using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScepterHand : MonoBehaviour
{
    public Transform ratKing; // Reference to rat king head
    public Transform shootPoint;
    public GameObject projectilePrefab;
    public float hoverDistance = 1.5f;
    public float fireInterval = 4f;
    public float swingCooldown = 6f;
    public float burstDelay = 0.3f;
    public int burstCount = 3;
    public float projectileSpeed = 6f;

    private bool isSwinging = false;
    private Vector2 originalOffset;
    private float fireTimer;
    private float swingTimer;
    private Vector2 swingTarget;
    private Vector2 returnPosition;

    void Start()
    {
        fireTimer = fireInterval;
        swingTimer = swingCooldown;
        originalOffset = new Vector2(-hoverDistance, 1.5f);
    }

    void Update()
    {
        if (!isSwinging)
        {
            transform.position = Vector2.Lerp(transform.position, (Vector2)ratKing.position + originalOffset, Time.deltaTime * 5f);

            fireTimer -= Time.deltaTime;
            swingTimer -= Time.deltaTime;

            if (fireTimer <= 0)
            {
                StartCoroutine(FireBurst());
                fireTimer = fireInterval;
            }
            else if (swingTimer <= 0)
            {
                StartCoroutine(SwingAttack());
                swingTimer = swingCooldown;
            }
        }
    }

    IEnumerator FireBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            Vector2 direction = (PlayerPos() - (Vector2)shootPoint.position).normalized;
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

            yield return new WaitForSeconds(burstDelay);
        }
    }

    IEnumerator SwingAttack()
    {
        isSwinging = true;
        swingTarget = PlayerPos();
        returnPosition = (Vector2)ratKing.position + originalOffset;

        while (Vector2.Distance(transform.position, swingTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, swingTarget, Time.deltaTime * 10f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // pause after swing

        while (Vector2.Distance(transform.position, returnPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, returnPosition, Time.deltaTime * 10f);
            yield return null;
        }

        isSwinging = false;
    }

    Vector2 PlayerPos() => GameObject.FindGameObjectWithTag("Player").transform.position;
}