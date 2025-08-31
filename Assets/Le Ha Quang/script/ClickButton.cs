using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClickButton : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }
    public void OnStartButtonClicked()
    {
        audioSource.PlayOneShot(audioSource.clip);
        SceneManager.LoadScene("Map scene");
    }

   
    
    public void OnClickSetting()
    {
        audioSource.PlayOneShot(audioSource.clip);
        SceneManager.LoadScene("Setting scene",LoadSceneMode.Additive);
    }
    
}
