using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public SceneInfo sceneInfo;
    
    public GameObject shieldUpgradeObject;
    public GameObject bulletsizeUpgradeObject;

    public GameObject FireRateUpgradePrefab;


    public GameObject bulletSizeIncreasePrefab;

    private List<string> unlockedUpgrades = new List<string>();

    private void Start()
    {
        ApplyUpgrades();
    }

    public void ApplyUpgrades()
    {
        // Uppdatera aktivering av objekt baserat på uppgraderingsstatus
        shieldUpgradeObject.SetActive(sceneInfo.hasShieldUpgrade);
        bulletsizeUpgradeObject.SetActive(sceneInfo.hasBulletsizeUpgrade);
        FireRateUpgradePrefab.SetActive(sceneInfo.hasFireRateUpgrade);

        // Kontrollera om BulletSizeUpgrade är upplåst och applicera den
        if (sceneInfo.hasBulletsizeUpgrade)
        {
            ApplyBulletSizeUpgrade();
        }

        // Kontrollera om FireRateUpgrade är upplåst och applicera den
        if (sceneInfo.hasFireRateUpgrade)
        {
            ApplyFireRateUpgrade();
        }
    }

    public void UnlockUpgrade(string upgradeName)
    {
        if (!unlockedUpgrades.Contains(upgradeName))
        {
            unlockedUpgrades.Add(upgradeName);
            sceneInfo.UnlockUpgrade(upgradeName);
            ApplyUpgrades();
        }
    }

    public bool HasUpgrade(string upgradeName)
    {
        return unlockedUpgrades.Contains(upgradeName);
    }

    public void UnlockShieldUpgrade()
    {
        sceneInfo.hasShieldUpgrade = true;
        ApplyUpgrades();
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UnlockBulletSizeUpgrade()
    {
        Debug.Log("Unlocking bullet size upgrade");
        sceneInfo.hasBulletsizeUpgrade = true;
        ApplyUpgrades();
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ApplyBulletSizeUpgrade()
    {
        if (bulletSizeIncreasePrefab != null)
        {
            Instantiate(bulletSizeIncreasePrefab);
        }
        else
        {
            Debug.LogWarning("BulletSizeIncrease prefab is not assigned!");
        }
    }
   public void unlockFireRateUpgrade()
{
    // Kolla om fireRate kan minskas (t.ex. så att det inte blir negativt eller under ett visst minimum)
    if (sceneInfo.fireRate > 0.1f) // Förhindra att fireRate blir för låg
    {
        sceneInfo.fireRate -= 0.1f;
        PlayerPrefs.SetFloat("fireRate", sceneInfo.fireRate);  // Uppdatera PlayerPrefs
    }
    else
    {
        Debug.Log("Maximum fire rate upgrade reached.");
    }

    ApplyUpgrades(); // Applicera uppgraderingen i UI:t
    upgradeCanvas.SetActive(false);
    Time.timeScale = 1f; // Fortsätt spelet
}
  private void ApplyFireRateUpgrade()
    {
        if (FireRateUpgradePrefab != null)
        {
            Instantiate(FireRateUpgradePrefab);
        }
        else
        {
            Debug.LogWarning("FireRateUpgrade prefab is not assigned!");
        }
    }
  
}
