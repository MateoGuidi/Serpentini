using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown screenModeDropdown;
    [SerializeField]
    private Button frenchButton;
    [SerializeField]
    private Button englishButton;
    [SerializeField] 
    private Button japaneseButton;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider volumeSlider;

    private const string VolumeParameter = "Master";
    private const string PlayerPrefKey = "SavedVolume";
    private const float minVolume = 0.0001f;
    private const float maxDecibels = 0f;
    private const float minDecibels = -80f;

    private void Start()
    {
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            LocalizationSettings.InitializationOperation.WaitForCompletion();
        }
        if (frenchButton != null)
            frenchButton.onClick.AddListener(() => ChangeLanguage("fr"));
        if (englishButton != null)
            englishButton.onClick.AddListener(() => ChangeLanguage("en"));
        if (japaneseButton != null)
            japaneseButton.onClick.AddListener(() => ChangeLanguage("ja"));
        int currentMode = GetCurrentScreenModeIndex();
        screenModeDropdown.value = currentMode;
        screenModeDropdown.onValueChanged.AddListener(ChangeScreenMode);
        if (masterMixer.GetFloat(VolumeParameter, out float currentVolume))
        {
            volumeSlider.value = Mathf.Pow(10, currentVolume / 20f);
        }
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public async void ChangeLanguage(string localeCode)
    {
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            await LocalizationSettings.InitializationOperation.Task;
        }

        Locale selectedLocale = null;
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == localeCode)
            {
                selectedLocale = locale;
                break;
            }
        }

        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
            Debug.Log($"Langue changée en : {localeCode}");
        }
        else
        {
            Debug.LogWarning($"Locale non trouvée pour : {localeCode}");
        }
    }

    public void ChangeScreenMode(int index)
    {
        switch (index)
        {
            case 0: // Plein écran
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
                break;

            case 1: // Fenêtré
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;
        }

        Debug.Log($"Mode d'affichage changé : {screenModeDropdown.options[index].text}");
    }

    private int GetCurrentScreenModeIndex()
    {
        return Screen.fullScreenMode switch
        {
            FullScreenMode.ExclusiveFullScreen => 0,
            FullScreenMode.FullScreenWindow => 1,
            FullScreenMode.Windowed => 2,
            _ => 1
        };
    }

    public void ChangeVolume(float sliderValue)
    {
        sliderValue = Mathf.Max(sliderValue, minVolume);
        float volumeDb = Mathf.Log10(sliderValue) * 20f;
        masterMixer.SetFloat(VolumeParameter, volumeDb);
        PlayerPrefs.SetFloat(PlayerPrefKey, sliderValue);
        PlayerPrefs.Save();
    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
