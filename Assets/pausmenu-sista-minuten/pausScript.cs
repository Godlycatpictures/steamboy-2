using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pausScript : MonoBehaviour
{
    [SerializeField]
    public SceneInfo sceneInfo;

    public GameObject SettingsThingy;
    // Start is called before the first frame update
    void Start()
    {
        SettingsThingy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �r den st�ngd?
            if (!SettingsThingy.activeSelf)
            {
                Time.timeScale = 0f;
                SettingsThingy.SetActive(true);

            } // om �ppen --> st�ng
            else
            {
                Time.timeScale = 1f;
                SettingsThingy.SetActive(false);

            }
        }
    }

    public void Quit()  // knappen f�r avlsuta
    {
        

        Application.Quit();
    }

    public void Resume() // knappen f�r restart
    {
        Time.timeScale = 1f;
        SettingsThingy.SetActive(false);
    }

    public void BackToMainMenu()  // ja
    {
        Debug.Log("startar om hela runnet f�rhoppningsvis");
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart() // starta om leveln (nya rum o s�nt)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
