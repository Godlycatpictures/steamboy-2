using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{

    public Button rerollButton; // Knappen för att rerolla uppgraderingar
    public UpgradeManager upgradeManager;
    public GameObject upgradeCanvas; // Canvas för uppgraderingsmenyn
     public SceneInfo sceneInfo;


    public List<Button> allUpgradeButtons; // Alla uppgraderingsknappar
    private List<Button> activeButtons = new List<Button>(); // De tre slumpmässigt valda knapparna

    private void Start()
    {
        HideAllButtons();
        rerollButton.gameObject.SetActive(false);
        rerollButton.onClick.AddListener(RerollUpgrades);
    }

    public void ShowUpgradeMenu()
    {
        HideAllButtons();
        SelectRandomUpgrades();

        // Visa reroll-knappen endast om spelaren har den och INTE har använt den
        bool canUseReRoll = upgradeManager.sceneInfo.hasReRoll && !upgradeManager.sceneInfo.hasUsedReRoll;
        rerollButton.gameObject.SetActive(canUseReRoll);

        upgradeCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideUpgradeMenu()
    {
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f; // Fortsätt spelet
    }

   private void SelectRandomUpgrades()
{
    List<Button> availableButtons = new List<Button>();

    // Lägg till alla knappar som INTE är upplåsta (förutom FireRate som kan uppgraderas flera gånger)
    foreach (Button button in allUpgradeButtons)
    {
        // Tillåt FireRate att alltid dyka upp även om den är "unlocked"
        if (IsFireRateUpgrade(button) || !IsUpgradeUnlockedForButton(button))
        {
            availableButtons.Add(button);
        }
        // Tillåt HP Upgrade att alltid dyka upp även om den är "unlocked"
       if(sceneInfo.numOfHearts < 10){
        if (IsHpUppgrade(button) || !IsUpgradeUnlockedForButton(button))
        {
            availableButtons.Add(button);
        }
       }
    }

    // Lägg till Re-Roll som ett alternativ om den inte är upplåstad
    if (!upgradeManager.sceneInfo.hasReRoll)
    {
        Button reRollUpgradeButton = allUpgradeButtons.Find(b => b.name == "ReRollUpgradeButton");
        if (reRollUpgradeButton != null && !availableButtons.Contains(reRollUpgradeButton))
        {
            availableButtons.Add(reRollUpgradeButton);
        }
    }

    activeButtons.Clear();

    float buttonSpacing = 50f;
    Vector3 startPosition = new Vector3(0f, 0f, 0f);

    // Välj 3 slumpmässiga unika knappar
    for (int i = 0; i < 3; i++)
    {
        if (availableButtons.Count == 0) break;

        int randomIndex = Random.Range(0, availableButtons.Count);
        Button selectedButton = availableButtons[randomIndex];
        activeButtons.Add(selectedButton);
        availableButtons.RemoveAt(randomIndex);

        // Positionera knappen
        RectTransform buttonRect = selectedButton.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = startPosition;
        startPosition.y -= buttonSpacing;

        selectedButton.gameObject.SetActive(true);
    }

    // Debugga vilka knappar som valts
    foreach (Button btn in activeButtons)
    {
        Debug.Log("Selected Upgrade: " + btn.name);
    }
}    private bool IsFireRateUpgrade(Button button)
    {
        int upgradeIndex = allUpgradeButtons.IndexOf(button);
        return upgradeIndex == 2; // Index 2 är FireRateUpgrade
    }
     private bool IsHpUppgrade(Button button)
    {
        int upgradeIndex = allUpgradeButtons.IndexOf(button);
        return upgradeIndex == 8; 
    }
    private bool IsUpgradeUnlockedForButton(Button button)
    {
        int upgradeIndex = allUpgradeButtons.IndexOf(button);

        switch (upgradeIndex)
        {
            case 0: return upgradeManager.sceneInfo.hasShieldUpgrade;
            case 1: return upgradeManager.sceneInfo.hasBulletsizeUpgrade;
            case 2: return upgradeManager.sceneInfo.hasFireRateUpgrade;
            case 3: return upgradeManager.sceneInfo.hasAutoFireUpgrade;
            case 4: return upgradeManager.sceneInfo.hasReRoll;
            case 5: return upgradeManager.sceneInfo.hasGhostCompanion; // lowercase 's' in sceneInfo // Add this to IsUpgradeUnlockedForButton
            case 6: return upgradeManager.sceneInfo.hasExplodingBullets;
            //case 7: return upgradeManager.sceneInfo.hasChainDamage; // Ny index för ChainDamage
            case 8: return upgradeManager.sceneInfo.hasHpUppgrade; // Ny index för HP Upgrade
            case 9 : return upgradeManager.sceneInfo.hasTinyUppgrade; // Ny index för Tiny Upgrade


            default: return false;
        }
    }

    private void HideAllButtons()
    {
        foreach (Button button in allUpgradeButtons)
        {
            button.gameObject.SetActive(false);      //DETTA KAN VARA EN BUGG, ÄR KOMMENTERAD JUST NU
        }
    }

   public void RerollUpgrades()
{
    if (!upgradeManager.sceneInfo.hasReRoll || upgradeManager.sceneInfo.hasUsedReRoll)
        return;

    Debug.Log("Re-Roll used! Getting new upgrades...");

    // Markera att reroll har använts så att knappen inte visas igen
    upgradeManager.sceneInfo.hasUsedReRoll = true;
    rerollButton.gameObject.SetActive(false);

    // Dölj nuvarande knappar och välj nya
    HideAllButtons();
    SelectRandomUpgrades();
}


   public void UnlockReRollUpgrade()
{
    upgradeManager.UnlockReRollUpgrade(); // Anropa UpgradeManager för att låsa upp Re-Roll
    HideUpgradeMenu(); // Stäng uppgraderingsmenyn
}
public void UnlockGhostCompanionUpgrade()
{
    upgradeManager.UnlockGhostCompanion();
    HideUpgradeMenu();
}
public void UnlockExplodingBullets()
{
    upgradeManager.UnlockExplodingBulletsUpgrade();
    HideUpgradeMenu();
}
public void UnlockChainDamage()
{
    upgradeManager.UnlockChainDamageUpgrade();
    HideUpgradeMenu();
}

}
