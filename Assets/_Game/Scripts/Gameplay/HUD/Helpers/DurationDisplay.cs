using TMPro;
using UnityEngine;

public class DurationDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    float elapsed;
    bool active = true;

    private void Update()
    {
        if (!active) return;
        elapsed += Time.deltaTime;
        timerText.text = FormatTime(elapsed);
    }

    public void Stop() => active = false;

    public float Elapsed => elapsed;

    string FormatTime(float t)
    {
        int m = (int)(t / 60);
        int s = (int)(t % 60);
        return $"{m:00}:{s:00}";
    }
}