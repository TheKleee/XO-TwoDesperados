using UnityEngine;
using System.Collections;

public class PopupAnimator : MonoBehaviour
{
    [SerializeField] float animDuration = 0.2f;
    [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public void Show(Transform target, System.Action onComplete = null)
    {
        StopAllCoroutines();
        target.localScale = Vector3.zero;
        StartCoroutine(Scale(target, Vector3.zero, Vector3.one, onComplete));
    }

    public void Hide(Transform target, System.Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(Scale(target, Vector3.one, Vector3.zero, onComplete));
    }

    IEnumerator Scale(Transform target, Vector3 from, Vector3 to, System.Action onComplete)
    {
        float elapsed = 0f;
        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animDuration);
            target.localScale = Vector3.LerpUnclamped(from, to, t);
            yield return null;
        }
        target.localScale = to;
        onComplete?.Invoke();
    }
}