using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float fireRate = 0.5f; // Time between shots
    private float nextFireTime = 0f; // When the next shot can be fired

    [SerializeField]
    public SceneInfo sceneInfo;  // Kontrollera att denna referens är kopplad via Inspector

 void Start()
{
    if (sceneInfo != null)
    {
        // Återställ fireRate till värdet från PlayerPrefs om det finns, annars använd standardvärde
        fireRate = PlayerPrefs.GetFloat("fireRate", 0.5f);  // Hämtar värdet från PlayerPrefs (standard är 0.5f)
        fireForce = sceneInfo.fireForce;
    }
    else
    {
        Debug.LogError("SceneInfo is not assigned in Weapon script!");
    }

    // Logga för att kontrollera initialisering
    Debug.Log("Start fireRate: " + fireRate);
}

    void Update()
    {
        // Uppdatera fireRate varje frame från SceneInfo
        if (sceneInfo != null)
        {
            fireRate = sceneInfo.fireRate;  // Uppdatera fireRate varje frame
        }

        // Kontrollera att fireRate faktiskt förändras vid varje knapptryckning
        Debug.Log("Updated fireRate: " + fireRate);

        // Om spelaren trycker på skjutknappen och tidpunkten är rätt
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate; // Ställ in nästa skott-tid
        }
    }

    public void Fire()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = firePoint.right * fireForce;
    }
}
