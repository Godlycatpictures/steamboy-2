using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public SceneInfo sceneInfo;
    public GameObject ghostCompanionPrefab;
    public GameObject activeGhost;
    public GameObject player;

    public GameObject shieldUpgradeObject;
    public GameObject bulletsizeUpgradeObject;
    public GameObject FireRateUpgradePrefab;
    public GameObject bulletSizeIncreasePrefab;

    private List<string> unlockedUpgrades = new List<string>();

    private void Start()
    {
        ApplyUpgrades();
    }
    void FixedUpdate()
    {
        if (player == null)
        {

            player = GameObject.FindGameObjectWithTag("Player");
            // Försök hitta och assigna uppgraderingsobjekten om de inte redan är satta
            if (shieldUpgradeObject == null)
                shieldUpgradeObject = GameObject.FindGameObjectWithTag("Shield");
            if (bulletsizeUpgradeObject == null)
                bulletsizeUpgradeObject = GameObject.FindGameObjectWithTag("BulletSize+");
            if (FireRateUpgradePrefab == null)
                FireRateUpgradePrefab = GameObject.FindGameObjectWithTag("FireRate");

        }
    
    }

    public void ApplyUpgrades()
    {
        if (shieldUpgradeObject != null)
            shieldUpgradeObject.SetActive(sceneInfo.hasShieldUpgrade);
        else
            Debug.LogWarning("ShieldUpgradeObject is not assigned!");

        if (bulletsizeUpgradeObject != null)
            bulletsizeUpgradeObject.SetActive(sceneInfo.hasBulletsizeUpgrade);
        else
            Debug.LogWarning("BulletsizeUpgradeObject is not assigned!");

        if (FireRateUpgradePrefab != null)
            FireRateUpgradePrefab.SetActive(sceneInfo.hasFireRateUpgrade);
        else
            Debug.LogWarning("FireRateUpgradePrefab is not assigned!");

        if (sceneInfo.hasBulletsizeUpgrade)
        {
            ApplyBulletSizeUpgrade();
        }

        if (sceneInfo.hasFireRateUpgrade)
        {
            ApplyFireRateUpgrade();
        }
        
        // Aktivera Re-Roll om spelaren har den
        if (sceneInfo.hasReRoll)
        {
            Debug.Log("Re-Roll Upgrade is available!");
        }
    }

    public bool HasUpgrade(string upgradeName)
    {
        return unlockedUpgrades.Contains(upgradeName);
    }

    public void UnlockShieldUpgrade()
    {
        if (!sceneInfo.hasShieldUpgrade)
        {
            sceneInfo.hasShieldUpgrade = true;
            shieldUpgradeObject.SetActive(true);
            unlockedUpgrades.Add("ShieldUpgrade");
            Debug.Log("Shield upgrade unlocked.");
        }

        CloseUpgradeMenu();
    }

    public void UnlockBulletSizeUpgrade()
    {
        if (!sceneInfo.hasBulletsizeUpgrade)
        {
            sceneInfo.hasBulletsizeUpgrade = true;
            bulletsizeUpgradeObject.SetActive(true);
            ApplyBulletSizeUpgrade();
            unlockedUpgrades.Add("BulletsizeUpgrade");
            Debug.Log("Bullet size upgrade unlocked.");
        }

        CloseUpgradeMenu();
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
            unlockedUpgrades.Add("FireRateUpgrade");
            ApplyFireRateUpgrade();
        }

        CloseUpgradeMenu();
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

    public void UnlockFullAutoUpgrade()
    {
        if (!sceneInfo.hasAutoFireUpgrade)
        {
            sceneInfo.hasAutoFireUpgrade = true;
            unlockedUpgrades.Add("FullAutoUpgrade");
            sceneInfo.fireRate *= 2;

            Weapon weapon = FindObjectOfType<Weapon>();
            if (weapon != null)
            {
                weapon.isAutoFireEnabled = true;
            }
        }

        CloseUpgradeMenu();
    }

    // Ny metod för att låsa upp Re-Roll
public void UnlockReRollUpgrade()
{
    if (!sceneInfo.hasReRoll)
    {
        sceneInfo.hasReRoll = true;
        sceneInfo.hasUsedReRoll = false; // Se till att spelaren kan använda den
        Debug.Log("Re-Roll upgrade unlocked!");
    }
    CloseUpgradeMenu();
}



    // Återställ Re-Roll när spelaren går upp i level
    public void ResetReRollOnLevelUp()
    {
        sceneInfo.hasUsedReRoll = false;
        Debug.Log("Re-Roll Reset on Level Up!");
    }

    private void CloseUpgradeMenu()
    {
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1f;
        sceneInfo.screenShake = true;
    }
    public void UnlockGhostCompanion()
    {
        if (!sceneInfo.hasGhostCompanion)
        {
            sceneInfo.hasGhostCompanion = true;
            
            // Instantiate ghost using same pattern as shield/bullet upgrades
            if (ghostCompanionPrefab != null)
            {
                activeGhost = Instantiate(ghostCompanionPrefab);
                activeGhost.GetComponent<GhostCompanion>().player = 
                    GameObject.FindGameObjectWithTag("Player").transform;
                activeGhost.SetActive(true); // Explicit activation
            }
            
            unlockedUpgrades.Add("GhostCompanion");
            Debug.Log("Ghost companion unlocked!");
        }
        CloseUpgradeMenu();
    }
    public void UnlockExplodingBulletsUpgrade()
{
    Debug.Log("Clicked Exploding Bullets button");

    if (!sceneInfo.hasExplodingBullets)
    {
        sceneInfo.hasExplodingBullets = true;
        unlockedUpgrades.Add("ExplodingBullets");
        
    }

    CloseUpgradeMenu();
}

public void UnlockChainDamageUpgrade()
{
    Debug.Log("Clicked chain Bullets button");

    if (!sceneInfo.hasChainDamage)
    {
        sceneInfo.hasChainDamage = true;
        unlockedUpgrades.Add("ChainDamage");
        Debug.Log("Chain Damage unlocked!");
    }

    CloseUpgradeMenu();
}
public void UnlockHpUppgrade(){

    Debug.Log("Clicked Hp Upgrade button");

    if(!sceneInfo.hasHpUppgrade)
    {
        sceneInfo.hasHpUppgrade = true;
        unlockedUpgrades.Add("HpUpgrade");
        sceneInfo.HpUp();
     
        Debug.Log("Hp Upgrade unlocked!");
    }
    CloseUpgradeMenu();


}
public void UnlockTinyUppgrade()
{
    if(!sceneInfo.hasTinyUppgrade)
    {
        Debug.Log("Clicked Tiny Upgrade button");
        sceneInfo.hasTinyUppgrade = true;
        unlockedUpgrades.Add("TinyUpgrade");
        Debug.Log("Tiny Upgrade unlocked!");
        if (player != null)
        {
            player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Scale down the player
        }
        else
        {
            Debug.LogWarning("Player GameObject is not assigned!");
        }
        
    }
    CloseUpgradeMenu();


}

}
