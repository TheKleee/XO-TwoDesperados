using DataXO;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        OrientationBridge.Unlock();
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion singleton />

    [Header("Background Music")]
    [SerializeField] AudioSource bgmSource; //Background music :)

    [Header("Sound effects")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip placement;
    [SerializeField] AudioClip strike;
    [SerializeField] AudioClip popup;
    [SerializeField] AudioClip hover;
    
    private void Start()
    {
        SettingsData settings = SaveManager.LoadSettings() ?? new SettingsData();
        SetMusic(settings.musicEnabled);
        SetSFX(settings.sfxEnabled);
    }

    public void SetMusic(bool enabled) => bgmSource.enabled = enabled;
    public void SetSFX(bool enabled) => sfxSource.enabled = enabled;

    void PlaySFX(AudioClip clip)
    {
        if (sfxSource.enabled)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayHover() => PlaySFX(hover);
    public void PlayButtonClick() => PlaySFX(buttonClick);
    public void PlayPlacement() => PlaySFX(placement);
    public void PlayStrike() => PlaySFX(strike);
    public void PlayPopup() => PlaySFX(popup);
}