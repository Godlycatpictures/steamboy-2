using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSizeIncrease : MonoBehaviour
{
    public float sizeMultiplier = 1.5f;  // Multiplikator för att öka storleken på kulan
    private List<GameObject> processedBullets = new List<GameObject>();  // För att hålla koll på vilka kulor som redan har ändrats

    void Update()
    {
        // Hitta alla objekt med taggen "Bullet"
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            // Om kulan inte redan har blivit behandlad
            if (!processedBullets.Contains(bullet))
            {
                // Hitta transform-komponenten för kulan
                Transform bulletTransform = bullet.transform;

                // Öka storleken på kulan med sizeMultiplier
                bulletTransform.localScale *= sizeMultiplier;

                // Lägg till kulan i listan så att den inte behandlas flera gånger
                processedBullets.Add(bullet);
            }
        }
    }
}
