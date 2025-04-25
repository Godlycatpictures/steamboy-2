using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spikeDamage;
    void Start()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {

    yield return new WaitForSeconds(0.7f);

        Vector2 spawnPosition = transform.position + new Vector3(0, 0.15f, 0); // Spawn attack above enemy
        Instantiate(spikeDamage, spawnPosition, Quaternion.identity);

    yield return new WaitForSeconds(1.3f);

    Destroy(gameObject);

    }
}
