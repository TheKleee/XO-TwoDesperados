using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StepCounter : MonoBehaviour
{
    [SerializeField] TMP_Text valueText;
    [SerializeField] Button incrementButton;
    [SerializeField] Button decrementButton;

    int value;
    int min;
    int max;

    public int Value => value;

    public void Init(int defaultVal, int minVal, int maxVal)
    {
        min = minVal;
        max = maxVal;
        value = defaultVal;
        Refresh();
    }

    public void Increment()
    {
        value = Mathf.Min(value + 1, max);
        Refresh();
    }

    public void Decrement()
    {
        value = Mathf.Max(value - 1, min);
        Refresh();
    }

    public void SetMax(int newMax)
    {
        max = newMax;
        value = Mathf.Clamp(value, min, max);
        Refresh();
    }

    void Refresh()
    {
        valueText.text = $"{value}";
        incrementButton.interactable = value < max;
        decrementButton.interactable = value > min;
    }
}