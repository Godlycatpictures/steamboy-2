using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;
    public GameObject PBullet;
  
    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.CompareTag("Enemy")) // Check if it's a bullet
    { 
        Destroy(gameObject);
    }if (collision.CompareTag("EnemyProjectile")) // Check if it's a bullet
    { 
        Destroy(gameObject);
        Destroy(PBullet); 

    }
        Destroy(gameObject);
    }
}
