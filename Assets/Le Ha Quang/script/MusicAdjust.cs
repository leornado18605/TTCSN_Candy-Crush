using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAdjust : MonoBehaviour
{
    public static MusicAdjust Instance { get; private set; }
    private AudioSource audioSource;
    //private AudioSource sfSource;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //sfSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("music") && PlayerPrefs.HasKey("soundeffect"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("music");
            //sfSource.volume = PlayerPrefs.GetFloat("soundeffect");
        }
        else
        {
            PlayerPrefs.SetFloat("music", 1f);
            PlayerPrefs.SetFloat("soundeffect", 1f);
        }
    }

    public void SetMusic(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("music", value);
    }

    public void SetSoundEffect(float value)
    {
        //sfSource.volume = value;
        PlayerPrefs.SetFloat("soundeffect", value);
        
    }
}
