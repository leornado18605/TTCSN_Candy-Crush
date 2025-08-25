using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : Singleton<AudioController>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioMusic;
    [SerializeField] private AudioSource audioSound;

    [Header("Slider")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    [Header("Button")]
    [SerializeField] private Button btnMusic;
    [SerializeField] private Button btnSound;
    private bool isClickMusic = false;
    private bool isClickSound = false;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textMusic;
    [SerializeField] private TextMeshProUGUI textSound;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip audioMenuGame;
    [SerializeField] private AudioClip audioGamePlay;
    [SerializeField] private AudioClip audioGameWin;
    [SerializeField] private AudioClip audioGameLose;

    private void Start()
    {
        btnMusic.onClick.AddListener(delegate
        {
            OnClickButtonMusic();
        });
        btnSound.onClick.AddListener(delegate
        {
            OnClickButtonSound();
        });

        UpdateMusic(PlayerPrefs.GetFloat("Music", 0f));
        UpdateSound(PlayerPrefs.GetFloat("Sound", 0f));

        sliderMusic.onValueChanged.AddListener(UpdateMusic);
        sliderSound.onValueChanged.AddListener(UpdateSound);
    }
    //Slider
    public void UpdateMusic(float val)
    {
        textMusic.text = ((int)val*100).ToString() + " %";
        sliderMusic.value = val;
        audioMusic.volume = val;
        PlayerPrefs.SetFloat("Music", val);
        PlayerPrefs.Save();
    }
    public void UpdateSound(float val)
    {
        textSound.text = ((int)val * 100).ToString() + " %";
        sliderSound.value = val;
        audioSound.volume = val;
        PlayerPrefs.SetFloat("Sound", val);
        PlayerPrefs.Save();
    }

    //Button
    public void OnClickButtonMusic()
    {
        if(isClickMusic)
        {
            isClickMusic = false;
            UpdateMusic(1f);
            return;
        }
        UpdateMusic(0f);
    }
    public void OnClickButtonSound()
    {
        if (isClickSound)
        {
            isClickSound = false;
            UpdateSound(1f);
            return;
        }
        UpdateSound(0f);
    }
    public void PlayAudioMenuGame()
    {
        audioMusic.clip = audioMenuGame;
        PlayMusic();
    }
    public void PlayAudioGamePlay()
    {
        audioMusic.clip = audioGamePlay;
        PlayMusic();
    }
    public void PlayAudioGameWin()
    {
        audioMusic.clip = audioGameWin;
        PlayMusic();
    }
    public void PlayAudioGameLose()
    {
        audioMusic.clip = audioGameLose;
        PlayMusic();
    }
    public void PlayMusic()
    {
        audioMusic.Play();
    }
    public void StopMusic()
    {
        audioMusic.Stop();
    }
    public void PlaySound(AudioClip clip)
    {
        audioSound.PlayOneShot(clip);
    }
}
