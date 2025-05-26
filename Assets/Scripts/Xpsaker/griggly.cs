using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class griggly : MonoBehaviour
{
    
    public GameObject chainEffectPrefab;
    public GameObject deathEffect;
    public GameObject[] gorePrefabs;
    public GameObject[] bloodSplats; // 🩸 Add blood splats array

    public int health = 1;
    public int xp;
    public int goreAmount = 5;
    public float goreSpreadForce = 5f;
    public float goreLifetime = 2f;
    public int bloodAmount = 3; // 🩸 How many blood splats spawn

    private xpChar xpCharacter;
    public SceneInfo sceneInfo;
    public bool gore;

    void Start()
    {
        xpCharacter = FindObjectOfType<xpChar>();

        if (xpCharacter == null)
            Debug.LogError("xpChar script not found in the scene!");

        if (sceneInfo == null)
        {
            sceneInfo = FindObjectOfType<SceneInfo>();
            if (sceneInfo == null)
                Debug.LogWarning("SceneInfo is not assigned to enemy!");
        }
    }

    void Update()
    {

        gore = sceneInfo.gore;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("DroneProjectile"))
        {
            if (sceneInfo != null && sceneInfo.hasChainDamage)
            {
                ApplyChainDamage();
            }

            TakeDamage();
        }
    }

    void TakeDamage()
    {
        health--;

        StartCoroutine(HitShake());

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (xpCharacter != null)
            xpCharacter.AddXP(xp);

        if (deathEffect != null && gore)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (gore)
        {
            SpawnGore();
            SpawnBlood(); // 🩸 New blood splat function
        }

        Destroy(gameObject);
    }

void SpawnGore()
{
    if (gorePrefabs == null || gorePrefabs.Length == 0)
        return;

    for (int i = 0; i < goreAmount; i++)
    {
        int randomIndex = Random.Range(0, gorePrefabs.Length);
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        GameObject gorePiece = Instantiate(gorePrefabs[randomIndex], transform.position, randomRotation);

        Rigidbody2D rb = gorePiece.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            rb.AddForce(randomDir * Random.Range(goreSpreadForce * 0.5f, goreSpreadForce), ForceMode2D.Impulse);

            rb.drag = Random.Range(3f, 7f);
            rb.angularDrag = Random.Range(3f, 7f);
        }

        if (Random.value < 0.5f)
        {
            StartCoroutine(FadeAndDestroy(gorePiece, goreLifetime));
        }
        else
        {
            StartCoroutine(StickyGore(gorePiece));
        }
    }
}


    void SpawnBlood()
    {
        if (bloodSplats == null || bloodSplats.Length == 0)
            return;

        for (int i = 0; i < bloodAmount; i++)
        {
            int randomIndex = Random.Range(0, bloodSplats.Length);
            GameObject blood = Instantiate(bloodSplats[randomIndex]);

            // Small random offset around death position
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            blood.transform.position = (Vector2)transform.position + randomOffset;

            // Random rotation
            blood.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            // Random scaling between 0.8 and 1.2
            float randomScale = Random.Range(0.8f, 1.2f);
            blood.transform.localScale = new Vector3(randomScale, randomScale, 1f);
        }
    }

    IEnumerator FadeAndDestroy(GameObject gorePiece, float lifetime)
    {
        SpriteRenderer sr = gorePiece.GetComponent<SpriteRenderer>();

        float elapsed = 0f;
        Color originalColor = sr.color;

        yield return new WaitForSeconds(lifetime);

        float fadeDuration = 1f;

        while (elapsed < fadeDuration)
        {
            if (sr != null)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gorePiece);
    }

    IEnumerator StickyGore(GameObject gorePiece)
    {
        Rigidbody2D rb = gorePiece.GetComponent<Rigidbody2D>();

        yield return new WaitForSeconds(0.5f);

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }
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
            if (chainEffectPrefab != null)
            {
                Vector3 start = transform.position;
                Vector3 end = closest.position;
                Vector3 direction = end - start;
                float distance = direction.magnitude;

                GameObject effect = Instantiate(chainEffectPrefab);
                effect.transform.position = (start + end) / 2;
                effect.transform.right = direction.normalized;
                effect.transform.localScale = new Vector3(distance, effect.transform.localScale.y, 1f);

                Destroy(effect, 0.3f);
            }

            griggly otherEnemy = closest.GetComponent<griggly>();
            if (otherEnemy != null)
            {
                otherEnemy.TakeDamageFromChain(chainDamage);
            }
        }
    }

    public void TakeDamageFromChain(int damage)
    {
        health -= damage;

        StartCoroutine(HitShake());

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator HitShake()
    {
        Vector3 originalPos = transform.position;
        float duration = 0.1f;
        float magnitude = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            transform.position = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}
