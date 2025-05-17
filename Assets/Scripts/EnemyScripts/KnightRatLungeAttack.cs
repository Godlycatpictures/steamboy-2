using System.Collections;
using UnityEngine;

public class KnightRatLungeAttack : MonoBehaviour
{
    public KnightRatAi knightRatAi;      // Reference to the AI script
    public Transform ratKnight;          // The knight GameObject to follow

    void Start()
    {
        StartCoroutine(Death());

        // Optional safety assignment if not manually assigned
        if (knightRatAi == null && ratKnight != null)
        {
            knightRatAi = ratKnight.GetComponent<KnightRatAi>();
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    void Update()
    {
        if (ratKnight != null && knightRatAi != null)
        {
            // Use facingDirection to apply appropriate horizontal offset
            float offsetX = 0.6f * knightRatAi.facingDirection;
            Vector2 offset = new Vector2(offsetX, -0.2f);
            transform.position = (Vector2)ratKnight.position + offset;
        }
    }
}