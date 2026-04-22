using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PopupAnimator))]
public class ThemePopup : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;
    [SerializeField] GameObject mapConfigPanel;
    [SerializeField] GameObject skinPickerPanel;
    [SerializeField] MapConfig mapConfig;
    [SerializeField] SkinPicker skinPicker;

    PopupAnimator pa;
    private void Awake() =>    
        pa = GetComponent<PopupAnimator>();
    
    public void Open()
    {
        AudioManager.instance.PlayPopup();
        popupPanel.SetActive(true);
        mapConfigPanel.SetActive(true);
        skinPickerPanel.SetActive(false);
        pa.Show(popupPanel.transform);
    }

    public void Close()
    {
        pa.Hide(popupPanel.transform, () =>
        {
            AudioManager.instance.PlayPopup();
            popupPanel.SetActive(false);
        });
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