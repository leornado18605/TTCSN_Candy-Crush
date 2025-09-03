using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartSetting : Singleton<StartSetting>
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnQuit;

    [SerializeField] private Image imgSliderMusic;
    [SerializeField] private Image imgSliderSound;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textMusic;
    [SerializeField] private TextMeshProUGUI textSound;

    [Header("Slider")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioMusic;
    [SerializeField] private AudioSource audioSound;

    [SerializeField] private AudioClip buttonDown;
    [SerializeField] private AudioClip buttonPress;

    private void Start()
    {
        btnClose.onClick.AddListener(() =>
        {
            PlaySoundButtonDown();
            settingPanel.SetActive(false);
        });
        if(btnQuit != null)
        {
            btnQuit.onClick.AddListener(() =>
            {
                PlaySoundButtonDown();
                Application.Quit();
            });
        }

        UpdateMusic(PlayerPrefs.GetFloat("Music", 0f));
        UpdateSound(PlayerPrefs.GetFloat("Sound", 0f));

        sliderMusic.onValueChanged.AddListener(UpdateMusic);
        sliderSound.onValueChanged.AddListener(UpdateSound);
    }

    public void UpdateMusic(float val)
    {
        textMusic.text = ((int)(val * 100)).ToString() + " %";
        sliderMusic.value = val;
        audioMusic.volume = val;
        PlaySoundButtonPress();

        if (val == 0)
        {
            imgSliderMusic.gameObject.SetActive(true);
        }
        else
        {
            imgSliderMusic.gameObject.SetActive(false);
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

        if (val == 0)
        {
            imgSliderSound.gameObject.SetActive(true);
        }
        else
        {
            imgSliderSound.gameObject.SetActive(false);
        }

        PlayerPrefs.SetFloat("Sound", val);
        PlayerPrefs.Save();
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
