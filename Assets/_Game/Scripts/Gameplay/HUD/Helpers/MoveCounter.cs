using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] TMP_Text[] moveCountTexts; // one per player in inspector

    public void RegisterMove(byte playerId, int count) =>
        moveCountTexts[playerId - 1].text = $"P{playerId}: {count}";
}