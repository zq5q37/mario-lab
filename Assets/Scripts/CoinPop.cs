using UnityEngine;
using System.Collections;

public class CoinPop : MonoBehaviour
{
    public float popHeight = 1.5f;   
    public float popSpeed = 4f;      

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + Vector3.up * popHeight;

        StartCoroutine(AnimateCoin());
    }

    IEnumerator AnimateCoin()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * popSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * popSpeed;
            transform.position = Vector3.Lerp(endPos, startPos, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
