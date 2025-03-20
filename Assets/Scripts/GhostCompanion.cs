using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCompanion : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 3f;
    public float shootingInterval = 2f;
    public GameObject homingBulletPrefab;
    public float bulletSpeed = 5f;
    public float shootingRange = 5f; // Avstånd där companion får skjuta
    public LayerMask enemyLayer; // Lager för fiender

    private float shootingTimer;

    void Update()
    {
        if (player == null) return;

        // Följ spelaren smidigt
        Vector3 targetPosition = player.position + new Vector3(1.5f, 1.5f, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Kolla om det finns fiender inom räckvidd
        if (EnemyInRange())
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer >= shootingInterval)
            {
                ShootHomingBullet();
                shootingTimer = 0f;
            }
        }
    }

    bool EnemyInRange()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, shootingRange, enemyLayer);
        return enemies.Length > 0;
    }

    void ShootHomingBullet()
    {
        if (homingBulletPrefab == null)
        {
            Debug.LogError("Missing bullet prefab in GhostCompanion!");
            return;
        }

        GameObject bullet = Instantiate(homingBulletPrefab, transform.position + (transform.up * 0.5f), Quaternion.identity);

        if (bullet == null)
        {
            Debug.LogError("Bullet instantiation failed!");
            return;
        }

        HomingBullet homingScript = bullet.GetComponent<HomingBullet>();
        if (homingScript != null)
        {
            homingScript.Initialize(bulletSpeed, 200f);
        }
        else
        {
            Debug.LogError("Missing HomingBullet script on bullet prefab!");
        }
    }

    // Rita ut räckvidden i scenen för debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
