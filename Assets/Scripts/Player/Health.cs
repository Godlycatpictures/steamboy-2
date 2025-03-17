using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public int health = 3;
    public int numOfHearts = 3;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [SerializeField]
    public SceneInfo sceneInfo;

    void Start()
    {
        numOfHearts = sceneInfo.numOfHearts;
        health = sceneInfo.health;  
        
    }

    void FixedUpdate()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++){

            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        sceneInfo.health = health;
        sceneInfo.numOfHearts = numOfHearts;
    }

    private void loseLife()
    {
        health--;
    }
    private void gainLife()
    {
        health++;
    }
    private void lifeUp()
    {
        numOfHearts++;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            loseLife();
        }
    }
}