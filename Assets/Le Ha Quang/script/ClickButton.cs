using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClickButton : MonoBehaviour
{
     AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnStartButtonClicked()
    {
        audioSource.PlayOneShot(audioClip);
        SceneManager.LoadScene("Map scene");
    }

    public void OnXClick()
    {
        audioSource.PlayOneShot(audioClip);
        SceneManager.UnloadSceneAsync("Setting scene");
    }
    
    public void OnClickSetting()
    {
        audioSource.PlayOneShot(audioClip);
        SceneManager.LoadScene("Setting scene",LoadSceneMode.Additive);
    }
    
}
