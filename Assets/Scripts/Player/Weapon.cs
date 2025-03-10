
using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float fireRate = 0.5f; // Time between shots
    private float nextFireTime = 0f; // When the next shot can be fired
    public bool mongo;

    [SerializeField]
    public SceneInfo sceneInfo;
    
    void Start()
    {
        fireForce = sceneInfo.fireForce;
        fireRate = sceneInfo.fireRate;

    }

    void Update()
    {
        sceneInfo.fireForce = fireForce;
        sceneInfo.fireRate = fireRate;

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate; // Set the next fire time
        }
    }

    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = firePoint.right * fireForce;
    }
}
