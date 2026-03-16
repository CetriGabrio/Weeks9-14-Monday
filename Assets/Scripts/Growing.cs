using System.Collections;
using UnityEngine;

public class growing : MonoBehaviour
{
    public AnimationCurve growthCurve;
    public float duration = 1f;
    public float maxScale = 1f;

    void Start()
    {
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float scaleValue = growthCurve.Evaluate(t) * maxScale;
            transform.localScale = Vector3.one * scaleValue;
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one * maxScale;
    }
}