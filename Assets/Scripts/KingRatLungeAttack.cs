using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingRatLungeAttack : MonoBehaviour
{
    public EnemyHeadController EnemyHeadController;      // Reference to the AI script
    public Transform ratKing;          // The king GameObject to follow

    void Start()
    {
        StartCoroutine(Death());

        // Optional safety assignment if not manually assigned
        if (EnemyHeadController == null && ratKing != null)
        {
            EnemyHeadController = ratKing.GetComponent<EnemyHeadController>();
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    void Update()
    {
        if (ratKing != null && ratKing != null)
        {
            transform.position = (Vector2)ratKing.position;
        }
    }
}