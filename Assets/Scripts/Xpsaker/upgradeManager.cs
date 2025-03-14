/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    public SceneInfo sceneInfo; // Reference to SceneInfo for saving upgrades
    public GameObject[] availablePrefabs; // List of prefabs to choose from
    
    private void Start()
    {
        if (sceneInfo == null)
        {
            Debug.LogError("SceneInfo is not assigned!");
        }
    }

    // Ändrad metod: Tar nu emot GameObject istället för index
    public void ChooseUpgrade(GameObject selectedPrefab)
    {
        // Lägg till den valda prefaben direkt i unlockedPrefabs
        sceneInfo.unlockedPrefabs.Add(selectedPrefab.name); // Spara namnet på prefaben istället

        Debug.Log($"Added {selectedPrefab.name} to unlocked prefabs!");
    }
    public void ApplyUpgrades(GameObject Player)
{
    foreach (GameObject prefabName in sceneInfo.unlockedPrefabs)
    {
        GameObject prefab = Array.Find(availablePrefabs, p => p.name == prefabName);
        if (prefab != null && Player != null)
        {
            Instantiate(prefab, Player.transform.position, Quaternion.identity);
        }
    }
}
}*/
