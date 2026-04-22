using DataXO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;

    SettingsData settings;

    private void Start()
    {
        settings = SaveManager.LoadSettings() ?? new SettingsData();
        musicToggle.isOn = settings.musicEnabled;
        sfxToggle.isOn = settings.sfxEnabled;

        musicToggle.onValueChanged.AddListener(OnMusicToggled);
        sfxToggle.onValueChanged.AddListener(OnSfxToggled);
    }

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

    public void MusicToggle() =>
        OnMusicToggled(musicToggle.isOn);

    public void SfxToggle()=>
        OnSfxToggled(sfxToggle.isOn);

    void OnMusicToggled(bool value)
    {
        AudioManager.instance.PlayButtonClick();
        settings.musicEnabled = value;
        SaveManager.SaveSettings(settings);
        AudioManager.instance.SetMusic(value);
    }

    void OnSfxToggled(bool value)
    {
        AudioManager.instance.PlayButtonClick();
        settings.sfxEnabled = value;
        SaveManager.SaveSettings(settings);
        AudioManager.instance.SetSFX(value);
    }
}