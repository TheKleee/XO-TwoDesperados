using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] DurationDisplay durationDisplay;
    [SerializeField] MoveCounter moveCounter;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    public void RegisterMove(byte playerId, int count) =>
        moveCounter.RegisterMove(playerId, count);

    public void StopMatch() =>
        durationDisplay.Stop();

    public float MatchDuration => durationDisplay.Elapsed;
}