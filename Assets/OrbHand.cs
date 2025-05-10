using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHand : MonoBehaviour
{
    public Transform ratKing;
    public Transform throwPoint;
    public GameObject orbPrefab;
    public float hoverDistance = 1.5f;
    public float throwInterval = 5f;
    public float orbSpeed = 7f;

    private float throwTimer;
    private Animator animator;
    private Vector2 originalOffset;

    void Start()
    {
        animator = GetComponent<Animator>();
        throwTimer = throwInterval;
        originalOffset = new Vector2(hoverDistance, 1.5f);
    }

    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, (Vector2)ratKing.position + originalOffset, Time.deltaTime * 5f);

        throwTimer -= Time.deltaTime;
        if (throwTimer <= 0)
        {
            StartCoroutine(ThrowOrb());
            throwTimer = throwInterval;
        }
    }

    IEnumerator ThrowOrb()
    {
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(0.2f); // sync with throw animation

        Vector2 direction = (PlayerPos() - (Vector2)throwPoint.position).normalized;
        GameObject orb = Instantiate(orbPrefab, throwPoint.position, Quaternion.identity);
        orb.GetComponent<Rigidbody2D>().velocity = direction * orbSpeed;

        yield return new WaitForSeconds(0.3f); // wait before resetting
        animator.SetBool("attacking", false);
    }

    Vector2 PlayerPos() => GameObject.FindGameObjectWithTag("Player").transform.position;
}