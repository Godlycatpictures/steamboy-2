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

    // Håll reda på vilka uppgraderingar som redan är upplåsta
    List<string> unlockedUpgrades = new List<string>
    {
        "ShieldUpgrade", "BulletsizeUpgrade", "FireRateUpgrade", "FullAutoUpgrade"
    };

    // Lägg till alla uppgraderingar som är upplåsta
    if (upgradeManager.sceneInfo.hasShieldUpgrade) unlockedUpgrades.Add("ShieldUpgrade");
    if (upgradeManager.sceneInfo.hasBulletsizeUpgrade) unlockedUpgrades.Add("BulletsizeUpgrade");
    if (upgradeManager.sceneInfo.hasFireRateUpgrade) unlockedUpgrades.Add("FireRateUpgrade");
    if (upgradeManager.sceneInfo.hasAutoFireUpgrade) unlockedUpgrades.Add("FullAutoUpgrade");

    // För att tillåta FireRateUpgrade flera gånger, vi kommer att särskilt hantera den
    int selectedCount = 0;
    while (selectedCount < 3)
    {
        if (availableButtons.Count == 0) break;

        int randomIndex = Random.Range(0, availableButtons.Count);
        Button selectedButton = availableButtons[randomIndex];

        // Kontrollera om uppgraderingen redan är upplåst för alla andra än FireRate
        if (IsUpgradeUnlockedForButton(selectedButton) && !IsFireRateUpgrade(selectedButton))
        {
            // Om uppgraderingen är upplåst och inte är FireRateUpgrade, fortsätt till nästa
            availableButtons.RemoveAt(randomIndex);
            continue;
        }

        // Lägg till knappen om den inte är upplåst, eller om den är FireRateUpgrade
        activeButtons.Add(selectedButton);
        selectedButton.gameObject.SetActive(true);

        RectTransform buttonRect = selectedButton.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = startPosition;

        startPosition.y -= buttonSpacing;

        availableButtons.RemoveAt(randomIndex);
        selectedCount++; // Inkrementera antalet valda uppgraderingar
    }
}

private bool IsFireRateUpgrade(Button button)
{
    // Kontrollera om knappen är för FireRateUpgrade
    int upgradeIndex = allUpgradeButtons.IndexOf(button);
    return upgradeIndex == 2; // Index 2 är FireRateUpgrade
}

private bool IsUpgradeUnlockedForButton(Button button)
{
    // Kontrollera vilken uppgradering knappen motsvarar
    int upgradeIndex = allUpgradeButtons.IndexOf(button);

    switch (upgradeIndex)
    {
        case 0: return upgradeManager.sceneInfo.hasShieldUpgrade;
        case 1: return upgradeManager.sceneInfo.hasBulletsizeUpgrade;
        case 2: return upgradeManager.sceneInfo.hasFireRateUpgrade;
        case 3: return upgradeManager.sceneInfo.hasAutoFireUpgrade;
        default: return false;
    }
}



    private void BindUpgradeButton(Button button, int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 0:
                button.onClick.AddListener(() => upgradeManager.UnlockShieldUpgrade());
                break;
            case 1:
                button.onClick.AddListener(() => upgradeManager.UnlockBulletSizeUpgrade());
                break;
            case 2:
                button.onClick.AddListener(() => upgradeManager.UnlockFireRateUpgrade());
                break;
            case 3:
                button.onClick.AddListener(() => upgradeManager.UnlockFullAutoUpgrade());
                break;
            default:
                Debug.LogError("Invalid upgrade index: " + upgradeIndex);
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
