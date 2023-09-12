using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _stepClip;
    [SerializeField] private AudioClip _shootClip;

    public void PlayStepClip()
    {
        _audioSource.clip = _stepClip;
        _audioSource.Play();
    }

    public void PlayShootClip()
    {
        _audioSource.clip = _shootClip;
        _audioSource.Play();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(SettingsPanel.EnvironmentVolumePPName)) _audioSource.volume = PlayerPrefs.GetFloat(SettingsPanel.EnvironmentVolumePPName);
        else _audioSource.volume = SettingsPanel.DefaultEnvironmentVolume;
    }

    private void OnEnable()
    {
        SettingsPanel.OnChangeEnvironmentVolume += ChangeVolume;
    }

    private void OnDisable()
    {
        SettingsPanel.OnChangeEnvironmentVolume -= ChangeVolume;
    }

    private void ChangeVolume(float newVolume)
    {
        _audioSource.volume = newVolume;
    }
}
