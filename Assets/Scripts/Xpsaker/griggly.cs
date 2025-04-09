using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class griggly : MonoBehaviour
{
    public int health = 1; // Enemy health
    public int xp; // XP to grant when enemy dies
    private xpChar xpCharacter; // Reference to xpChar script
    
    public GameObject deathEffect;

    public SceneInfo sceneInfo; // Drag-and-drop via Inspector!

    void Start()
    {
        xpCharacter = FindObjectOfType<xpChar>();

        if (xpCharacter == null)
        {
            Debug.LogError("xpChar script not found in the scene!");
        }

        if (sceneInfo == null)
        {
            sceneInfo = FindObjectOfType<SceneInfo>();
            if (sceneInfo == null)
                Debug.LogWarning("SceneInfo is not assigned to enemy!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("DroneProjectile"))
        {
            TakeDamage();

            if (sceneInfo != null && sceneInfo.hasChainDamage)
            {
                ApplyChainDamage();
            }
        }
    }

    void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (xpCharacter != null)
        {
            xpCharacter.AddXP(xp);
        }

        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void ApplyChainDamage()
    {
        float chainRange = 5f;
        int chainDamage = 1;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == gameObject) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist && dist <= chainRange)
            {
                closestDist = dist;
                closest = enemy.transform;
            }
        }

        if (closest != null)
        {
            // Rita blixt (DebugLine för nu – kan bytas mot riktig VFX)
            Debug.DrawLine(transform.position, closest.position, Color.yellow, 0.5f);

            // Skada närmaste fiende
            griggly otherEnemy = closest.GetComponent<griggly>();
            if (otherEnemy != null)
            {
                otherEnemy.TakeDamageFromChain(chainDamage);
            }
        }
    }

    // Extra metod för att tillåta chain-skada utan att aktivera kedjereaktion
    public void TakeDamageFromChain(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
}
