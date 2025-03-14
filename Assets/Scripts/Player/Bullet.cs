using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;
    public GameObject impactEffect;
    public GameObject bulletPrefab; // Prefab för att skapa nya kulor

    private UpgradeManager upgradeManager;

    void Awake()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();

        if (upgradeManager != null && upgradeManager.HasUpgrade("BulletDuplication"))
        {
            DuplicateBullet();
        }

        Destroy(gameObject, life);
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void DuplicateBullet()
    {
        int duplicateCount = 2; 
        float spreadAngle = 15f;

        for (int i = 0; i < duplicateCount; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);
            newBullet.transform.Rotate(0, 0, randomAngle);
        }
    }
}
