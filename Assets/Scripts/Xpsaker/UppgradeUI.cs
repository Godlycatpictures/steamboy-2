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

    // Sätt startposition för knapparna
    float buttonSpacing = 50f; // Avstånd mellan knapparna, justera efter behov
    Vector3 startPosition = new Vector3(0f, 0f, 0f); // Startposition för första knappen

    // Välj tre knappar slumpmässigt och aktivera dem
    for (int i = 0; i < 3; i++)
    {
        if (availableButtons.Count == 0) break; // Om det inte finns fler att välja

        int randomIndex = Random.Range(0, availableButtons.Count);
        Button selectedButton = availableButtons[randomIndex];

        activeButtons.Add(selectedButton);
        selectedButton.gameObject.SetActive(true); // Visa knappen

        // Dynamiskt placera knappen genom att justera RectTransform
        RectTransform buttonRect = selectedButton.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = startPosition;

        // Justera startposition för nästa knapp
        startPosition.y -= buttonSpacing; // Om du använder VerticalLayoutGroup
        // startPosition.x -= buttonSpacing; // Om du använder HorizontalLayoutGroup

        availableButtons.RemoveAt(randomIndex); // Ta bort den valda knappen från listan
    }
}


    private void BindUpgradeButton(Button button, int upgradeIndex)
    {
        // Här väljer vi vilken uppgradering som knappen ska representera
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
