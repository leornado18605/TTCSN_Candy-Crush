using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        GirdCandy.Instance.IsBusy = true;
        pannelSetting.transform.position = pannelSetting.transform.position + new Vector3(0, 10, 0);
        pannelSetting.SetActive(true);
        pannelSetting.transform.DOMove(pannelSetting.transform.position + new Vector3(0, -10, 0), 0.5f);
    }

    public void OnBack()
    {
        pannelSetting.transform.DOMove(pannelSetting.transform.position + new Vector3(0, 10, 0), 0.5f).OnComplete(() =>
        {
            pannelSetting.SetActive(false);
            pannelSetting.transform.position = pannelSetting.transform.position + new Vector3(0, -10, 0);
            GirdCandy.Instance.IsBusy = false;
        });
    }
    public void OnQuitLevel()
    {
        pannelSetting.transform.DOMove(pannelSetting.transform.position + new Vector3(0, 10, 0), 0.5f).OnComplete(() =>
        {
            pannelSetting.SetActive(false);
            pannelSetting.transform.position = pannelSetting.transform.position + new Vector3(0, -10, 0);
            MainGame.SetActive(false);
            UIController.Instance.loseUI.OnLose();
            GirdCandy.Instance.IsBusy = false;
        });
    }    
}
