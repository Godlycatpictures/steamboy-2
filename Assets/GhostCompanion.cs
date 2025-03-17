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

    private float shootingTimer;

    void Update()
    {
        if (player == null) return;

        // Smooth follow player
        Vector3 targetPosition = player.position + new Vector3(1.5f, 1.5f, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Shooting logic
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            ShootHomingBullet();
            shootingTimer = 0f;
        }
    }

void ShootHomingBullet()
{
    // Debug: Verify prefab assignment
    if (homingBulletPrefab == null)
    {
        Debug.LogError("Missing bullet prefab in GhostCompanion!");
        return;
    }

    // Debug: Verify spawn position
    Debug.Log($"Trying to spawn bullet at {transform.position}");
    
    try
    {
        GameObject bullet = Instantiate(
            homingBulletPrefab,
            transform.position + (transform.up * 0.5f), // Offset forward
            Quaternion.identity
        );
        
        // Debug: Verify instantiation
        if (bullet == null)
        {
            Debug.LogError("Bullet instantiation failed!");
            return;
        }
        Debug.Log($"Bullet spawned successfully at {bullet.transform.position}");

        // Initialize bullet components
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
    catch (System.Exception e)
    {
        Debug.LogError($"Bullet spawn error: {e.Message}");
    }
}
}
