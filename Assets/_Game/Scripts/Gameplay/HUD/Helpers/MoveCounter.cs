using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] TMP_Text[] moveCountTexts; // one per player in inspector

    int[] moveCounts;

    private void Start()
    {
        byte playerCount = MapBuilder.instance.playerCount;
        moveCounts = new int[playerCount];

        for (int i = 0; i < moveCountTexts.Length; i++)
            moveCountTexts[i].gameObject.SetActive(i < playerCount);
    }

    public void RegisterMove(byte playerId) =>
        moveCountTexts[playerId - 1].text = $"P{playerId}: {++moveCounts[playerId - 1]}";
}