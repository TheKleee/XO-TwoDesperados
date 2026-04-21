using UnityEngine;
using System.Collections;

public class Strike : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Vector3 target;

    public void Init(Vector3 startPos, Vector3 endPos)
    {
        transform.position = startPos;
        target = endPos;
        StartCoroutine(StrikeMove());
    }

    IEnumerator StrikeMove()
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
    }
}