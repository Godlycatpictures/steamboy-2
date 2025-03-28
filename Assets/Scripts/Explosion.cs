using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public SceneInfo sceneInfo;

    void Start()
    {
        
        FindObjectOfType<CameraScript>().StartShake();
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        
    yield return new WaitForSeconds(0.8f);

    Destroy(gameObject);

    }
}