using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicControl : MonoBehaviour
{
    [SerializeField] Slider musicAdjust;
    [SerializeField] Slider sfAdjust;


    void Start()
    {
        OnMusicChange();
        OnSoundEffectChange();
        
    }

    private void ChangeMusic()
    {
        MusicAdjust.Instance.SetMusic(musicAdjust.value);
       
    }

    private void OnMusicChange()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            musicAdjust.value = PlayerPrefs.GetFloat("music");
        }
        else
        {
            musicAdjust.onValueChanged.AddListener(value => ChangeMusic());
        }
    }

    public void ChangeSoundEffect()
    {
        MusicAdjust.Instance.SetSoundEffect(sfAdjust.value);

    }

    private void OnSoundEffectChange()
    {
        if (PlayerPrefs.HasKey("soundeffect"))
        {
            sfAdjust.value = PlayerPrefs.GetFloat("soundeffect");
        }
        else
        {
            sfAdjust.onValueChanged.AddListener(value => ChangeSoundEffect());
        }
    }

   
    

}
