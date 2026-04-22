using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemePopup : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;
    [SerializeField] GameObject mapConfigPanel;
    [SerializeField] GameObject skinPickerPanel;
    [SerializeField] MapConfig mapConfig;
    [SerializeField] SkinPicker skinPicker;

    public void Open()
    {
        AudioManager.instance.PlayPopup();
        popupPanel.SetActive(true);
        mapConfigPanel.SetActive(true);
        skinPickerPanel.SetActive(false);
    }

    public void Close()
    {
        AudioManager.instance.PlayPopup();
        popupPanel.SetActive(false);
    }

    public void OnBackClicked()
    {
        AudioManager.instance.PlayButtonClick();
        ((ISkinData)MapBuilder.instance).ClearSkins();
        skinPickerPanel.SetActive(false);
        mapConfigPanel.SetActive(true);
    }

    public void OnConfigConfirmed()
    {
        AudioManager.instance.PlayButtonClick();
        mapConfig.Apply();
        skinPicker.Init(mapConfig.Players);
        mapConfigPanel.SetActive(false);
        skinPickerPanel.SetActive(true);
    }

    public void OnStartClicked()
    {
        AudioManager.instance.PlayButtonClick();
        skinPicker.AssignDefaults();
        MapBuilder.instance.playerCount = mapConfig.Players;
        Close();
        SceneManager.LoadScene(1);
    }
}