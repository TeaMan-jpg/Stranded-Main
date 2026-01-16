using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shake : MonoBehaviour
{
    public float duration = 1f;
    public bool start = false;
    public AnimationCurve curve;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (start) {
            start = false;
            StartCoroutine(Shaking());
            
        }
    }

    IEnumerator Shaking() {
       Vector3 startPosition = transform.localPosition;
       float elapsed = 0.0f;
        Console.WriteLine("Initial: " + startPosition);
        while (elapsed < duration) {

            
            elapsed += Time.deltaTime;
            float strength = curve.Evaluate(elapsed / duration);
            transform.localPosition = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.localPosition = startPosition;
        Console.WriteLine("Final: " + startPosition);

    }
}
