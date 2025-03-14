/*using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;
public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager upgradeManager; // Reference to UpgradeManager
    public Button[] upgradeButtons; // Buttons for each upgrade
    public GameObject[] prefabsToChooseFrom; // Prefabs to be assigned to the buttons

    private void Start()
    {
        // Set up buttons to call ChooseUpgrade() when clicked
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i; // Store index to pass to the button click event
            // När en knapp klickas, skickas GameObject till ChooseUpgrade
            upgradeButtons[i].onClick.AddListener(() => OnUpgradeButtonClicked(prefabsToChooseFrom[index]));
        }
    }

    // Metod som anropas när knappen klickas
    private void OnUpgradeButtonClicked(GameObject selectedPrefab)
    {
        upgradeManager.ChooseUpgrade(selectedPrefab); // Skicka GameObject till UpgradeManager
    }
}*/
