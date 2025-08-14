using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicControl : MonoBehaviour
{
    [SerializeField] Slider musicAdjust;
    //[SerializeField] Slider sfAdjust;


    void Start()
    {
        if (PlayerPrefs.HasKey("music")&&PlayerPrefs.HasKey("soundeffect"))
        {
            OnLoad();
        }
        else
        {
            ChangeMusic();
        }
    }

    private void ChangeMusic()
    {
        MusicAdjust.Instance.SetMusic(musicAdjust.value);
        //MusicAdjust.Instance.SetSoundEffect(sfAdjust.value);
    }

    private void OnLoad()
    {
        musicAdjust.value = PlayerPrefs.GetFloat("music");
        //sfAdjust.value = PlayerPrefs.GetFloat("soundeffect");
    }
    

}
