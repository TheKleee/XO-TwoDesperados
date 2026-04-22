using UnityEngine;
using System.Collections;

public class Strike : MonoBehaviour
{
    [SerializeField] float duration = 0.1f;
    [SerializeField] LineRenderer lineRenderer;

    [Header("Effects:")]
    [SerializeField] GameObject trailEffect;
    [SerializeField] float spawnInterval = 0.01f;
    [SerializeField] float effectLifespan = 0.1f;

    public void Init(Vector3 startPos, Vector3 endPos, System.Action onComplete)
    {
        startPos.x += .5f;
        startPos.z += .5f; //Adjusting for the offset

        endPos.x += .5f;
        endPos.z += .5f; //Same as with startPos xD

        // Push start and end outward to make the line longer
        Vector3 dir = (endPos - startPos).normalized;
        float extend = 0.5f;
        startPos -= dir * extend;
        endPos += dir * extend;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos); // start collapsed
        StartCoroutine(DrawLine(startPos, endPos, onComplete));
    }

    IEnumerator DrawLine(Vector3 start, Vector3 end, System.Action onComplete)
    {
        AudioManager.instance.PlayStrike();
        float elapsed = 0f;
        float lastSpawnTime = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 current = Vector3.Lerp(start, end, t);
            lineRenderer.SetPosition(1, current);
            
            if (elapsed - lastSpawnTime >= spawnInterval)
            {
                current.y += .2f;
                lastSpawnTime = elapsed;
                var p = Instantiate(trailEffect, current, Quaternion.identity);
                Destroy(p, effectLifespan);
            }

            yield return null;
        }
        lineRenderer.SetPosition(1, end);
        onComplete?.Invoke();
    }
}