using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firerateuppgrade : MonoBehaviour
{
     public SceneInfo sceneInfo;
    // Start is called before the first frame update
    void Awake()
    {
        if (sceneInfo != null)
        {
            sceneInfo.fireRate -= 0.1f;
        }
        else
        {
            Debug.LogError("sceneInfo is not assigned.");
        }
    }

    // Update is called once per frame
   
}
