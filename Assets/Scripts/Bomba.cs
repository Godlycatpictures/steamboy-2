using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
  public float life = 3;
      public GameObject collosionEffect;
        public GameObject PBullet;
  
    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      if (collision.CompareTag("Enemy"))
        {
            
            Instantiate(collosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject); // Förstör missilen
            Destroy(PBullet); 
           
        }

        if (collision.CompareTag("EnemyProjectile"))
        {
            
            Instantiate(collosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject); // Förstör missilen
            Destroy(PBullet); 
           
        }
    }
}
