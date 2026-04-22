using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text durationText;

    public void OpenWin(byte playerId, float duration)
    {
        AudioManager.instance.PlayPopup();
        resultText.text = $"Player {playerId} Wins!";
        durationText.text = $"Duration: {duration:F1}s";
        panel.SetActive(true);
    }

    public void OpenDraw(float duration)
    {
        AudioManager.instance.PlayPopup();
        resultText.text = "Draw!";
        durationText.text = $"Duration: {duration:F1}s";
        panel.SetActive(true);
    }

    public void OnRetry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void OnExit()
    {
        ((ISkinData)MapBuilder.instance).ClearSkins();
        SceneManager.LoadScene(0);
    }
}