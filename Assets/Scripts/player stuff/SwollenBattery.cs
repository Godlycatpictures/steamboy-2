using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwollenBattery : MonoBehaviour
{

    [SerializeField]
    public SceneInfo sceneInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sceneInfo.health <= 0) 
        {
            Debug.Log("atleast say you got killet by the boss rat");
            DEATH();
        }
    }

    public void DEATH()
    {
        SceneManager.LoadScene("Main menu");
    }
}
