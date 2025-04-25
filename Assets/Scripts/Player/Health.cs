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

    public float invincibilityTime = 0.5f;
    private bool isInvincible = false;

    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float flashSpeed = 0.5f;

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

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < health ? fullHeart : emptyHeart;
            hearts[i].enabled = i < numOfHearts;
        }

        sceneInfo.health = health;
        numOfHearts = sceneInfo.numOfHearts;
    }

    private void loseLife()
    {
        health--;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile") && !isInvincible)
        {
            StartCoroutine(InvincibilityDuration());
        }
    }

    private IEnumerator InvincibilityDuration()
    {
        isInvincible = true;

        loseLife();

        float elapsed = 0f;
        Color originalColor = playerSprite.color;

        while (elapsed < invincibilityTime)
        {
            playerSprite.color = new Color(1f, 1f, 1f, 0.5f); // half-transparent
            yield return new WaitForSeconds(flashSpeed);
            playerSprite.color = originalColor;
            yield return new WaitForSeconds(flashSpeed);

            elapsed += flashSpeed * 2;
        }

        playerSprite.color = originalColor;
        isInvincible = false;
    }
}
