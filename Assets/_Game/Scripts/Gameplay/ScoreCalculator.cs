using DataXO;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    StatsData stats;

    private void Start() =>
        stats = SaveManager.LoadStats() ?? new StatsData();

    public void RecordWin(byte playerId, float duration)
    {
        stats.totalGames++;
        stats.totalDuration += duration;
        stats.playerWins[playerId - 1]++;
        SaveManager.SaveStats(stats);
    }

    public void RecordDraw(byte[] activePlayers, float duration)
    {
        stats.totalGames++;
        stats.totalDuration += duration;
        stats.draws++;
        foreach (var id in activePlayers)
            stats.playerDraws[id - 1]++;
        SaveManager.SaveStats(stats);
    }

    public float AverageDuration =>
        stats.totalGames == 0 ? 0 : stats.totalDuration / stats.totalGames;

    public StatsData GetStats() => stats;
}