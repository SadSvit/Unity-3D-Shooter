using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuScript : MenuScript
{
    [Header("General settings")]
    [SerializeField] private MenuScript previousMenu;

    [Header("Dropdown settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Slider settings")]
    [SerializeField] private Slider gameVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [Header("Text settings")]
    [SerializeField] private TMP_Text gameVolumePercent;
    [SerializeField] private TMP_Text musicVolumePercent;

    [Header("AudioMixer settings")]
    [SerializeField] private AudioMixer audioMixer;

    public void Start()
    {
        MenuName = MenuNames.SettingsMenu;
    }
    public void OpenSetingsMenu()
    {
        menuManager.OpenMenu(MenuNames.SettingsMenu);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetGameVolume()
    {
        if (musicVolumeSlider.value <= -40)
        {
            audioMixer.SetFloat("Game", -80);
        }
        else
        {
            audioMixer.SetFloat("Game", gameVolumeSlider.value);

            //изменения текста, показывающего процент громкости игры
            gameVolumePercent.text = Mathf.RoundToInt(GetPercentage(gameVolumeSlider.value, -40, 0)).ToString();
        }
    }

    public void SetMusicVolume()
    {
        if (musicVolumeSlider.value <= -40)
        {
            audioMixer.SetFloat("Music", -80);
        }
        else
        {
            audioMixer.SetFloat("Music", musicVolumeSlider.value);

            //изменения текста, показывающего процент громкости музыки
            musicVolumePercent.text = Mathf.RoundToInt(GetPercentage(musicVolumeSlider.value, -40, 0)).ToString();
        }
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
        {
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        }
        else
        {
            qualityDropdown.value = 0;
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySetingPreference", qualityDropdown.value);
    }

    public void BackToPreviousMenu()
    {
        menuManager.OpenMenu(previousMenu.MenuName);
    }

    float GetPercentage(float value, float min, float max)
    {
        // Убедимся, что значение находится в пределах от min до max
        value = Mathf.Clamp(value, min, max);
        // Вычисляем процент
        float percentage = (value - min) / (max - min) * 100;

        return percentage;
    }

}
