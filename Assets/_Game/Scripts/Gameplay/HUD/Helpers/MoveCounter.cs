using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] TMP_Text[] moveCountTexts; // one per player in inspector
    
    private void Start()
    {
        byte playerCount = MapBuilder.instance.playerCount;
        for (int i = 0; i < moveCountTexts.Length; i++)
            moveCountTexts[i].gameObject.SetActive(i < playerCount);
    }

    public void RegisterMove(byte playerId, int count) =>
        moveCountTexts[playerId - 1].text = $"P{playerId}: {count}";
}