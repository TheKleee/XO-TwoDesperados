using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] ThemePopup themePopup;
    [SerializeField] StatsPopup statsPopup;
    [SerializeField] SettingsPopup settingsPopup;
    [SerializeField] ExitConfirmPopup exitPopup;

    public void OnPlayClicked()
    {
        AudioManager.instance.PlayButtonClick();
        themePopup.Open();
    }
    public void OnStatsClicked()
    {
        AudioManager.instance.PlayButtonClick();
        statsPopup.Open();
    }
    public void OnSettingsClicked()
    {
        AudioManager.instance.PlayButtonClick();
        settingsPopup.Open();
    }
    public void OnExitClicked()
    {
        AudioManager.instance.PlayButtonClick();
        exitPopup.Open();
    }
}