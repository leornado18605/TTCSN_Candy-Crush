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
    [SerializeField] private Button btnRung;
    private bool isClickMusic = false;
    private bool isClickSound = false;
    private bool isClickRung = false;

    [Header("Image")]
    [SerializeField] private Image imgMusic;
    [SerializeField] private Image imgSound;
    [SerializeField] private Image imgRung;
    [SerializeField] private Image imgSliderMusic;
    [SerializeField] private Image imgSliderSound;


    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textMusic;
    [SerializeField] private TextMeshProUGUI textSound;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip audioMenuGame;
    [SerializeField] private AudioClip audioGamePlay;
    [SerializeField] private AudioClip audioGameWin;
    [SerializeField] private AudioClip audioGameLose;

    [SerializeField] private AudioClip buttonDown;
    [SerializeField] private AudioClip buttonPress;

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
        btnRung.onClick.AddListener(delegate
        {
            OnClickRung();
        });

        UpdateMusic(PlayerPrefs.GetFloat("Music", 0f));
        UpdateSound(PlayerPrefs.GetFloat("Sound", 0f));

        sliderMusic.onValueChanged.AddListener(UpdateMusic);
        sliderSound.onValueChanged.AddListener(UpdateSound);
    }
    //Slider
    public void UpdateMusic(float val)
    {
        textMusic.text = ((int)(val*100)).ToString() + " %";
        sliderMusic.value = val;
        audioMusic.volume = val;
        PlaySoundButtonPress();

        if (val == 0)
        {
            imgSliderMusic.gameObject.SetActive(true);
            imgMusic.gameObject.SetActive(true);
        }
        else
        {
            imgSliderMusic.gameObject.SetActive(false);
            imgMusic.gameObject.SetActive(false);
        }

        PlayerPrefs.SetFloat("Music", val);
        PlayerPrefs.Save();
    }
    public void UpdateSound(float val)
    {
        textSound.text = ((int)(val * 100)).ToString() + " %";
        sliderSound.value = val;
        audioSound.volume = val;
        PlaySoundButtonPress();

        if(val == 0)
        {
            imgSliderSound.gameObject.SetActive(true);
            imgSound.gameObject.SetActive(true);
        }
        else
        {
            imgSliderSound.gameObject.SetActive(false);
            imgSound.gameObject.SetActive(false);
        }

        PlayerPrefs.SetFloat("Sound", val);
        PlayerPrefs.Save();
    }

    //Button
    public void OnClickButtonMusic()
    {
        PlaySoundButtonDown();
        if (isClickMusic)
        {
            isClickMusic = false;
            UpdateMusic(1f);
        }
        else
        {
            isClickMusic = true;
            UpdateMusic(0f);
        }
    }
    public void OnClickButtonSound()
    {
        PlaySoundButtonDown();
        if (isClickSound)
        {
            isClickSound = false;
            UpdateSound(1f);
        }
        else
        {
            isClickSound = true;
            UpdateSound(0f);
        }

    }
    public void OnClickRung()
    {
        PlaySoundButtonDown();
        if (isClickRung)
        {
            isClickRung = false;
            imgRung.gameObject.SetActive(false);
        }
        else
        {
            isClickRung = true;
            imgRung.gameObject.SetActive(true);
        }
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

    public void PlaySoundButtonDown()
    {
        PlaySound(buttonDown);
    }
    public void PlaySoundButtonPress()
    {
        PlaySound(buttonPress);
    }
}
