using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] skins; // same 6 skins

    public void Init(byte playerId)
    {
        byte skinId = MapBuilder.instance.playerSkinMap[playerId];
        spriteRenderer.sprite = skins[skinId];
    }
}