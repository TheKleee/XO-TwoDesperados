using System.Collections;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] skins; // same 6 skins

    [SerializeField] float popDuration = 0.1f;
    [SerializeField] AnimationCurve popCurve; // overshoot curve in inspector

    public void Init(byte playerId)
    {
        byte skinId = MapBuilder.instance.playerSkinMap[playerId];
        spriteRenderer.sprite = skins[skinId];
        StartCoroutine(PopIn());
    }

    IEnumerator PopIn()
    {
        float elapsed = 0f;
        transform.localScale = Vector3.zero;
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = popCurve.Evaluate(elapsed / popDuration);
            transform.localScale = Vector3.one * t;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}