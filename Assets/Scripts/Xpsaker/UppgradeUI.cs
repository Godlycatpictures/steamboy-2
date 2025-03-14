using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public GameObject upgradeCanvas; // Canvas för uppgraderingsmenyn

    public List<Button> allUpgradeButtons; // Alla uppgraderingsknappar
    private List<Button> activeButtons = new List<Button>(); // De tre slumpmässigt valda knapparna

    private void Start()
    {
        HideAllButtons();
    }

    public void ShowUpgradeMenu()
    {
        HideAllButtons(); // Göm alla knappar först
        SelectRandomUpgrades(); // Välj tre slumpmässiga

        upgradeCanvas.SetActive(true);
        Time.timeScale = 0f; // Pausa spelet
    }

    public void HideUpgradeMenu()
    {
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f; // Fortsätt spelet
    }

   private void SelectRandomUpgrades()
   {
       List<Button> availableButtons = new List<Button>(allUpgradeButtons);
       activeButtons.Clear();

       float buttonSpacing = 50f; 
       Vector3 startPosition = new Vector3(0f, 0f, 0f);

       for (int i = 0; i < 3; i++)
       {
           if (availableButtons.Count == 0) break; 

           int randomIndex = Random.Range(0, availableButtons.Count);
           Button selectedButton = availableButtons[randomIndex];

           activeButtons.Add(selectedButton);
           selectedButton.gameObject.SetActive(true); 

           RectTransform buttonRect = selectedButton.GetComponent<RectTransform>();
           buttonRect.anchoredPosition = startPosition;

           startPosition.y -= buttonSpacing; 

           availableButtons.RemoveAt(randomIndex); 
       }
   }

    private void BindUpgradeButton(Button button, int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 0:
                button.onClick.AddListener(() => upgradeManager.UnlockUpgrade("ShieldUpgrade"));
                break;
            
            default:
                break;
        }
    }

    private void HideAllButtons()
    {
        foreach (Button button in allUpgradeButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
}
