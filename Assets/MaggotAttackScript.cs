using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaggotAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Death());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Death()
    {

    yield return new WaitForSeconds(0.3f);

    Destroy(gameObject);

    }
}
