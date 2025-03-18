using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public bool isAutoFireEnabled = false;  // Flagga för att hantera full-auto

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;

    private float nextFireTime = 0f; // When the next shot can be fired

    [SerializeField]
    public SceneInfo sceneInfo;  // Referens till SceneInfo

 void Update()
{
    float currentFireRate = sceneInfo.fireRate; // Hämtar den aktuella fireRate från SceneInfo

    if (isAutoFireEnabled)
    {
        // Om full-auto är aktiverat och knappen hålls nere
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + currentFireRate; // Använd den uppdaterade fireRate
        }
    }
    else
    {
        // Om inte full-auto, skjuta en gång per knapptryckning
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + currentFireRate; // Använd den uppdaterade fireRate
        }
    }
}

    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = firePoint.right * fireForce;
    }
}
