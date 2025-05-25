using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public bool isAutoFireEnabled = false;  // Flagga för att hantera full-auto
    public GameObject ExplodingBulletPrefab; // Referens till prefab för exploderande kulor
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioSource gunshot1;
    public AudioSource gunshot2;
    public AudioSource gunshot3;
    public float fireForce = 1f;

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
        if(sceneInfo.hasExplodingBullets)
        {
            // Om spelaren har uppgraderingen för exploderande kulor, använd den
            GameObject bullet = Instantiate(ExplodingBulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = firePoint.right * fireForce;
        }
        else
        {
          var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = firePoint.right * fireForce;
        }

        PlayGunshot();
    }

    public void PlayGunshot() {
        int randomIndex = Random.Range(0, 3); 

        switch (randomIndex)
        {
            case 0:
                gunshot1.Play();
                break;
            case 1:
                gunshot2.Play(); 
                break;
            case 2:
                gunshot3.Play(); 
                break;
        }
    }
}
