using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject {

    public int health; //M�ngden liv kvar
    public int numOfHearts; //Max antal hj�rtan

    public int currentCoins; //Antalet mone som spelaren har

    public float fireForce; //hastigheten av skottet
    public float fireRate; //Ska bli mindre f�r att den ska skjuta snabbare

    public int damageModifier = 1;

    public int randmap;

    public int xp = 0;
    public int level = 1;

    // H�ller reda p� vilka rum som har blivit rensade
    public List<bool> roomsCleared = new List<bool>();
}