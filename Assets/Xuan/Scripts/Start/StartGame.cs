using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnQuit;
    [SerializeField] private Button btnSetting;

    [SerializeField] private GameObject settingPanel;

    private void Start()
    {
        StartSetting.Instance.PlayMusic();

        btnPlay.onClick.AddListener(() =>
        {
            StartSetting.Instance.PlaySoundButtonDown();
            StartSetting.Instance.StopMusic();
            SceneManager.LoadScene("Xuan");
        });
        btnQuit.onClick.AddListener(() =>
        {
            StartSetting.Instance.PlaySoundButtonDown();
            StartSetting.Instance.StopMusic();
            Application.Quit();
        });
        btnSetting.onClick.AddListener(() =>
        {
            StartSetting.Instance.PlaySoundButtonDown();
            settingPanel.SetActive(true);
        });
    }
}
