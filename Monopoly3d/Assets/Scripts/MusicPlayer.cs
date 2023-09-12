using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _musics;
    private int _currentAudioClipId;

    private void Start()
    {
        if (PlayerPrefs.HasKey(SettingsPanel.MusicVolumePPName)) _audioSource.volume = PlayerPrefs.GetFloat(SettingsPanel.MusicVolumePPName);
        else _audioSource.volume = SettingsPanel.DefaultMusicVolume;
        _currentAudioClipId = Random.Range(0, _musics.Count);
        ChangeAudioClip();
    }

    private void ChangeAudioClip()
    {
        if (_currentAudioClipId == _musics.Count - 1) _currentAudioClipId = 0;
        else _currentAudioClipId++;
        StartCoroutine(PlayAudioClip(_musics[_currentAudioClipId]));
    }

    private IEnumerator PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
        float timer = audioClip.length;
        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            yield return null;
        }
        ChangeAudioClip();
    }

    private void OnEnable()
    {
        SettingsPanel.OnChangeMusicVolume += ChangeVolume;
    }

    private void OnDisable()
    {
        SettingsPanel.OnChangeMusicVolume -= ChangeVolume;
    }

    private void ChangeVolume(float newVolume)
    {
        _audioSource.volume = newVolume;
    }
}
