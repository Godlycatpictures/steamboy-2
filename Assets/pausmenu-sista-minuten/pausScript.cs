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
            // är den stängd?
            if (!SettingsThingy.activeSelf)
            {
                Time.timeScale = 0f;
                SettingsThingy.SetActive(true);

            } // om öppen --> stäng
            else
            {
                Time.timeScale = 1f;
                SettingsThingy.SetActive(false);

            }
        }
    }

    public void Quit()  // knappen för avlsuta
    {
        

        Application.Quit();
    }

    public void Resume() // knappen för restart
    {
        Time.timeScale = 1f;
        SettingsThingy.SetActive(false);
    }

    public void BackToMainMenu()  // ja
    {
        Debug.Log("startar om hela runnet förhoppningsvis");
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart() // starta om leveln (nya rum o sånt)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
