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

    private void Start()
    {
        btnPlay.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Xuan");
        });
        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        btnSetting.onClick.AddListener(() =>
        {
            
        });
    }
}
