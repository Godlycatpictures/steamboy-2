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
        Debug.Log("IT HIT");
    if (collision.CompareTag("Enemy")) // Check if it's a bullet
    { 
        Debug.Log("Enemy hit by bullet!");
        Destroy(gameObject);
        Destroy(PBullet); 

    }
    }
}
