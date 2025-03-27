using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {
        Debug.Log("I AM ALIVE");
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {

    yield return new WaitForSeconds(5f);

    //Destroy(gameObject);

    }
}
