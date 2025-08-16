using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button btnSetting; 
    [SerializeField] private Button btnQuitLevel; 
    [SerializeField] private Button btnBack;

    [SerializeField] private GameObject pannelSetting;
    [SerializeField] private GameObject MainGame;

    private void Start()
    {
        btnSetting.onClick.AddListener(delegate
        {
            OnSetting();
        });
        btnBack.onClick.AddListener(delegate
        {
            OnBack();
        });
        btnQuitLevel.onClick.AddListener(delegate
        {
            OnQuitLevel();
        });
    }

    public void OnSetting()
    {
        pannelSetting.SetActive(true);
    }

    public void OnBack()
    {
        pannelSetting.SetActive(false);
    }
    public void OnQuitLevel()
    {
        pannelSetting.SetActive(false);
        MainGame.SetActive(false);
    }    
}
