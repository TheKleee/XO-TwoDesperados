using DataXO;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PopupAnimator))]
public class StatsPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text totalGamesText;
    [SerializeField] TMP_Text[] playerWinsTexts; // one per player
    [SerializeField] TMP_Text drawsText;
    [SerializeField] TMP_Text avgDurationText;

    ScoreCalculator scoreCalculator;

    PopupAnimator pa;
    private void Awake() =>
        pa = GetComponent<PopupAnimator>();

    private void Start() =>
        scoreCalculator = FindFirstObjectByType<ScoreCalculator>();

    public void Open()
    {
        Refresh();
        AudioManager.instance.PlayPopup();
        panel.SetActive(true);
        pa.Show(panel.transform);
    }

    public void Close()
    {
        pa.Hide(panel.transform, () =>
        {
            AudioManager.instance.PlayPopup();
            panel.SetActive(false);
        });
    }

    void Refresh()
    {
        StatsData stats = scoreCalculator.GetStats();
        totalGamesText.text = $"{stats.totalGames}";
        drawsText.text = $"{stats.draws}";
        avgDurationText.text = $"{scoreCalculator.AverageDuration:F1}s";
        for (int i = 0; i < playerWinsTexts.Length; i++)
            playerWinsTexts[i].text = $"{stats.playerWins[i]}";
    }
}