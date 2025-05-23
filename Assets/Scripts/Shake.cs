using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; // The player or object to follow
    public float shakeDuration = 0.5f;
    public AnimationCurve shakeCurve;
    private Vector3 shakeOffset = Vector3.zero; // Stores shake effect
   
    private bool isShaking = false;
    public bool screenShake;

    public SceneInfo sceneInfo;

    private void Start(){

        StartCoroutine(AssignPlayer());

    }

    void Update()
    {
        screenShake = sceneInfo.screenShake;

        // Follow the player normally, with shake effect applied
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10) + shakeOffset;
        }
    }

    public void StartShake()
    {
        if (!isShaking && screenShake) // Prevent multiple shakes overlapping
        {
            StartCoroutine(ShakeRoutine());
        }
    }

    private IEnumerator ShakeRoutine()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = shakeCurve.Evaluate(elapsedTime / shakeDuration);
            shakeOffset = Random.insideUnitSphere * strength;

            yield return null;
        }

        // Reset shake offset after shaking ends
        shakeOffset = Vector3.zero;
        isShaking = false;
    }

    private IEnumerator AssignPlayer()
    {
        if (target == null)
        {

        target = GameObject.FindGameObjectWithTag("Player").transform;

        }
        
        yield return new WaitForSeconds(1f);

        StartCoroutine(AssignPlayer());
    }
}