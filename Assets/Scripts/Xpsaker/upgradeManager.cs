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
        shieldUpgradeObject.SetActive(sceneInfo.hasShieldUpgrade);
        bulletsizeUpgradeObject.SetActive(sceneInfo.hasBulletsizeUpgrade);
        FireRateUpgradePrefab.SetActive(sceneInfo.hasFireRateUpgrade);

        if (sceneInfo.hasBulletsizeUpgrade)
        {
            ApplyBulletSizeUpgrade();
        }

        if (sceneInfo.hasFireRateUpgrade)
        {
            ApplyFireRateUpgrade();
        }
    }

    public bool HasUpgrade(string upgradeName)
    {
        return unlockedUpgrades.Contains(upgradeName);
    }

    public void UnlockShieldUpgrade()
    {
        if (!sceneInfo.hasShieldUpgrade) // Se till att det inte redan är upplåst
        {
            sceneInfo.hasShieldUpgrade = true;
            shieldUpgradeObject.SetActive(true);
            Debug.Log("Shield upgrade unlocked.");
        }

        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UnlockBulletSizeUpgrade()
    {
        if (!sceneInfo.hasBulletsizeUpgrade)
        {
            sceneInfo.hasBulletsizeUpgrade = true;
            bulletsizeUpgradeObject.SetActive(true);
            ApplyBulletSizeUpgrade();
            Debug.Log("Bullet size upgrade unlocked.");
        }

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

    public void UnlockFireRateUpgrade()
    {
        if (sceneInfo.fireRate > 0.1f)
        {
            sceneInfo.fireRate -= 0.1f;
            PlayerPrefs.SetFloat("fireRate", sceneInfo.fireRate);
            Debug.Log("Fire rate decreased to: " + sceneInfo.fireRate);
        }
        else
        {
            Debug.Log("Maximum fire rate upgrade reached.");
        }

        if (!sceneInfo.hasFireRateUpgrade)
        {
            sceneInfo.hasFireRateUpgrade = true;
            FireRateUpgradePrefab.SetActive(true);
            ApplyFireRateUpgrade();
        }

        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
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
