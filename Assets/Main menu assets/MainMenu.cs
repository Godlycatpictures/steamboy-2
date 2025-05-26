using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string mainMenuTag = "MainMenuUI";
    public string settingsTag = "SettingsUI";
    public SceneInfo sceneInfo;
    public void PlayGame() {
        if (sceneInfo != null)
        {
            sceneInfo.ResetSceneInfo();  // Återställ all data när spelet startar
        }
        SceneManager.LoadScene("Simon Stuff 1");
    }

    public void OptionsGame() {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(mainMenuTag)) // Kontrollera om barnet har rätt tagg
            {
                child.gameObject.SetActive(false); // Göm objektet
            }

            if (child.CompareTag(settingsTag)) // Kontrollera om barnet har rätt tagg
            {
                child.gameObject.SetActive(true); // Visa objektet
            }
        }
    }

        public void BackToMainMenuGame() {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(mainMenuTag)) // Kontrollera om barnet har rätt tagg
            {
                child.gameObject.SetActive(true); // Visa objektet
            }

            if (child.CompareTag(settingsTag)) // Kontrollera om barnet har rätt tagg
            {
                child.gameObject.SetActive(false); // Göm objektet
            }
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
