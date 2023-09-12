using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _environmentSlider;
    [SerializeField] private Slider _mouseSensitivitySlider;

    public static float DefaultMusicVolume { get; } = 0.5f;
    public static float DefaultEnvironmentVolume { get; } = 0.5f;
    public static float DefaultMouseSensitivity { get; } = 0.5f;
    public static string MusicVolumePPName { get; } = "MusicVolume";
    public static string EnvironmentVolumePPName { get; } = "EnvironmentVolume";
    public static string MouseSensitivityPPName { get; } = "MouseSensitivity";

    private void Start()
    {
        _backButton.onClick.AddListener(Close);
        _musicSlider.onValueChanged.AddListener(ChangeMusicSliderValue);
        _environmentSlider.onValueChanged.AddListener(ChangeEnvironmentSliderValue);
        _mouseSensitivitySlider.onValueChanged.AddListener(ChangeMouseSensitivitySliderValue);

        if(PlayerPrefs.HasKey(MusicVolumePPName)) _musicSlider.value = PlayerPrefs.GetFloat(MusicVolumePPName);
        else _musicSlider.value = DefaultMusicVolume;

        if (PlayerPrefs.HasKey(EnvironmentVolumePPName)) _environmentSlider.value = PlayerPrefs.GetFloat(EnvironmentVolumePPName);
        else _environmentSlider.value = DefaultEnvironmentVolume;

        if (PlayerPrefs.HasKey(MouseSensitivityPPName)) _mouseSensitivitySlider.value = PlayerPrefs.GetFloat(MouseSensitivityPPName);
        else _mouseSensitivitySlider.value = DefaultMouseSensitivity;
    }

    private void ChangeMusicSliderValue(float newValue)
    {
        OnChangeMusicVolume?.Invoke(newValue);
        PlayerPrefs.SetFloat(MusicVolumePPName, _musicSlider.value);
    }

    private void ChangeEnvironmentSliderValue(float newValue)
    {
        OnChangeEnvironmentVolume?.Invoke(newValue);
        PlayerPrefs.SetFloat(EnvironmentVolumePPName, _environmentSlider.value);
    }

    private void ChangeMouseSensitivitySliderValue(float newValue)
    {
        OnChangeMouseSensitivity?.Invoke(newValue);
        PlayerPrefs.SetFloat(MouseSensitivityPPName, _mouseSensitivitySlider.value);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    public delegate void ChangeMusicVolume(float newValue);
    public static event ChangeMusicVolume OnChangeMusicVolume;

    public delegate void ChangeEnvironmentVolume(float newValue);
    public static event ChangeEnvironmentVolume OnChangeEnvironmentVolume;

    public delegate void ChangeMouseSensitivity(float newValue);
    public static event ChangeMouseSensitivity OnChangeMouseSensitivity;
}
