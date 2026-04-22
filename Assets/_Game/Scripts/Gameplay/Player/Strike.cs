using UnityEngine;
using System.Collections;

public class Strike : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Vector3 target;

    public void Init(Vector3 startPos, Vector3 endPos, System.Action onComplete)
    {
        startPos.x += .5f;
        startPos.z += .5f; //Adjusting for the offset
        transform.position = startPos;

        endPos.x += .5f;
        endPos.z += .5f; //Same as with startPos xD
        target = endPos;

        StartCoroutine(StrikeMove(onComplete));
    }

    IEnumerator StrikeMove(System.Action onComplete)    
    {
        AudioManager.instance.PlayStrike();
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );
            yield return null;
        }
        transform.position = target;
        onComplete?.Invoke();
    }
}