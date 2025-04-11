using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {

    yield return new WaitForSeconds(0.5f);

    Destroy(gameObject);

    }
}
