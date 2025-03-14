using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;
    public GameObject impactEffect;
    private UpgradeManager upgradeManager;

    void Awake()
    {
        Destroy(gameObject, life);

        // Hämta UpgradeManager från scenen
        upgradeManager = FindObjectOfType<UpgradeManager>();

        // 🔹 Kolla om Bullet Duplication är aktiv innan vi duplicerar
        if (upgradeManager != null && upgradeManager.HasUpgrade("BulletDuplication"))
        {
            DuplicateBullet();
        }
        else if (upgradeManager == null)
        {
            Debug.LogWarning("UpgradeManager not found in the scene!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void DuplicateBullet()
    {
        // 🔹 Skapar en ny bullet men ser till att den inte duplicerar sig själv igen
        GameObject duplicate = Instantiate(gameObject, transform.position, transform.rotation);
        Bullet bulletScript = duplicate.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // 🔴 Inaktivera duplication på den nya skottet för att undvika oändlig loop
            Destroy(bulletScript);
        }
    }
}
