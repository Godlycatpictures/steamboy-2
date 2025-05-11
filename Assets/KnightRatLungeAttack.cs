using System.Collections;
using UnityEngine;

public class KnightRatLungeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {

    yield return new WaitForSeconds(0.3f);

    Destroy(gameObject);

    }
}