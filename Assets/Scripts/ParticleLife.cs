using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLife : MonoBehaviour
{
public float life = 5;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        waiter();
    }

    private IEnumerator waiter()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
