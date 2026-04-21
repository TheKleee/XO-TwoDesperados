using UnityEngine;

public class ExitConfirmPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public void Open()
    {
        AudioManager.instance.PlayPopup();
        panel.SetActive(true);
    }

    public void Close()
    {
        AudioManager.instance.PlayPopup();
        panel.SetActive(false);
    }

    public void OnConfirm()
    {
        AudioManager.instance.PlayButtonClick();
        Application.Quit();
    }

    public void OnCancel() => Close();
}