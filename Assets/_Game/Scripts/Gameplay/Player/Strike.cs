using UnityEngine;
using System.Collections;

public class Strike : MonoBehaviour
{
    [SerializeField] float duration = 0.1f;
    [SerializeField] LineRenderer lineRenderer;

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
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            lineRenderer.SetPosition(1, Vector3.Lerp(start, end, t));
            yield return null;
        }
        lineRenderer.SetPosition(1, end);
        onComplete?.Invoke();
    }
}