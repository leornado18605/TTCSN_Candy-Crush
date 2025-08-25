using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnBack;

    private void Start()
    {
        btnNext.onClick.AddListener(delegate
        {
            OnNext();
        });
        btnBack.onClick.AddListener(delegate
        {
            OnBack();
        });
    }
    public void OnWin()
    {
        winPanel.SetActive(true);
    }
    public void OnNext()
    {
        winPanel.SetActive(false);
        StartGame.Instance.NextLevel();
        AudioController.Instance.PlayAudioMenuGame();
    }
    public void OnBack()
    {
        winPanel.SetActive(false);
        AudioController.Instance.PlayAudioMenuGame();
    }
}
