using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbHand : MonoBehaviour
{
    public GameObject orbPrefab;
    public float throwCooldown = 3f;
    public Animator animator;

    private Transform player;
    private float lastThrowTime;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (Time.time > lastThrowTime + throwCooldown)
        {
            StartCoroutine(ThrowOrb());
            lastThrowTime = Time.time;
        }
    }

    System.Collections.IEnumerator ThrowOrb()
    {
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(0.3f); // windup

        Instantiate(orbPrefab, transform.position, Quaternion.identity)
            .GetComponent<Rigidbody2D>()
            .velocity = (player.position - transform.position).normalized * 10f;

        animator.SetBool("attacking", false);
    }
}