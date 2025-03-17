using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinProjectile : MonoBehaviour
{
   public float life = 3;
   public GameObject impactEffect;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
