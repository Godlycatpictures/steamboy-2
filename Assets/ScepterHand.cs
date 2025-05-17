using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScepterHand : MonoBehaviour
{
    public enum AttackType { Melee, Burst }
    public float meleeRange = 3f;
    public GameObject projectilePrefab;
    public int burstCount = 5;
    public float burstInterval = 0.1f;
    public float cooldown = 4f;

    private Transform head;
    private Transform player;
    private float lastAttackTime;
    private bool isDetached = false;

    void Start()
    {
        head = transform.parent;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (Time.time > lastAttackTime + cooldown)
        {
            AttackType choice = Random.value > 0.5f ? AttackType.Melee : AttackType.Burst;
            StartCoroutine(choice == AttackType.Melee ? MeleeAttack() : BurstAttack());
            lastAttackTime = Time.time;
        }
    }

    System.Collections.IEnumerator MeleeAttack()
    {
        isDetached = true;
        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position;

        float t = 0;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        t = 0;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(targetPos, head.position, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }

        isDetached = false;
    }

    System.Collections.IEnumerator BurstAttack()
    {
        for (int i = 0; i < burstCount; i++)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity)
                .GetComponent<UrchinProjectile>().Initialize(dir);
                
            yield return new WaitForSeconds(burstInterval);
        }
    }
}
