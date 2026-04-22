using UnityEngine;

[RequireComponent(typeof(PopupAnimator))]
public class ExitConfirmPopup : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [Header("WebGL Quit:"),SerializeField] bool webGLQuit;
    
    PopupAnimator pa;
    private void Awake() =>
        pa = GetComponent<PopupAnimator>();

    public void Open()
    {
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

    public void OnConfirm()
    {
        AudioManager.instance.PlayButtonClick();
        if (webGLQuit)
            WebGLBridge.Quit();
        else
            Application.Quit();
    }

    public void OnCancel() => Close();
}