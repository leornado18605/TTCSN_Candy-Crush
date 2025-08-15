using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAdjust : MonoBehaviour
{
    public static MusicAdjust Instance { get; private set; }
    [SerializeField]public AudioSource audioSource;
    [SerializeField]public AudioSource sfSource;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        MusicChange();
        SoundEffectChange();
    }

    private void SoundEffectChange()
    {
        if (PlayerPrefs.HasKey("soundeffect"))
        {
            sfSource.volume = PlayerPrefs.GetFloat("soundeffect");

        }
        else
        {
            PlayerPrefs.SetFloat("soundeffect", 1f);

        }
    }

    private void MusicChange()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("music");
        }
        else
        {
            PlayerPrefs.SetFloat("music", 1f);
        }
    }

    public void SetMusic(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("music", value);
    }

    public void SetSoundEffect(float value)
    {
        sfSource.volume = value;
        PlayerPrefs.SetFloat("soundeffect", value);
        
    }
}
