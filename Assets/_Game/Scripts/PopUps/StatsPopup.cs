using DataXO;
using TMPro;
using UnityEngine;

public class StatsPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text totalGamesText;
    [SerializeField] TMP_Text[] playerWinsTexts; // one per player
    [SerializeField] TMP_Text drawsText;
    [SerializeField] TMP_Text avgDurationText;

    ScoreCalculator scoreCalculator;

    private void Start() =>
        scoreCalculator = FindFirstObjectByType<ScoreCalculator>();

    public void Open()
    {
        Refresh();
        AudioManager.instance.PlayPopup();
        panel.SetActive(true);
    }

    public void Close()
    {
        AudioManager.instance.PlayPopup();
        panel.SetActive(false);
    }

    void Refresh()
    {
        StatsData stats = scoreCalculator.GetStats();
        totalGamesText.text = $"Games Played: {stats.totalGames}";
        drawsText.text = $"Draws: {stats.draws}";
        avgDurationText.text = $"Avg Duration: {scoreCalculator.AverageDuration:F1}s";
        for (int i = 0; i < playerWinsTexts.Length; i++)
            playerWinsTexts[i].text = $"P{i + 1} Wins: {stats.playerWins[i]}";
    }
}