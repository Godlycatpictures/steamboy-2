using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject {

    public int health; //Mängden liv kvar
    public int numOfHearts; //Max antal hjärtan

    public int currentCoins; //Antalet mone som spelaren har

    public float fireForce; //hastigheten av skottet
    public float fireRate; //Ska bli mindre för att den ska skjuta snabbare

    public int damageModifier = 1;

    public int randmap;

    // Håller reda på vilka rum som har blivit rensade
    public List<bool> roomsCleared = new List<bool>();
}