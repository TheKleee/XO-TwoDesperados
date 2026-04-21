using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemePopup : MonoBehaviour
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

    // Called by skin picker buttons in inspector
    public void OnStartClicked()
    {
        AudioManager.instance.PlayButtonClick();
        Close();
        SceneManager.LoadScene("GameScene");
    }
}