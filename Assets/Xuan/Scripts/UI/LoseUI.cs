using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private GameObject pannelLose;
    [SerializeField] private GameObject message;
    [SerializeField] private GameObject menuRetry;

    [SerializeField] private Button btnRetry;
    [SerializeField] private Button btnBack;
    [SerializeField] private TextMeshProUGUI textScore;

    private void Start()
    {
        btnRetry.onClick.AddListener(delegate
        {
            Retry();
        });
        btnBack.onClick.AddListener(delegate
        {
            Back();
        });
    }

    public void OnLose()
    {
        pannelLose.SetActive(true);
        message.SetActive(true);
        
        DOVirtual.DelayedCall(0.7f, () =>
        {
            message.SetActive(false);
            menuRetry.SetActive(true);

            menuRetry.transform.position = menuRetry.transform.position + new Vector3(0, 10, 0);
            menuRetry.transform.DOMove(menuRetry.transform.position + new Vector3(0, -10, 0), 0.5f);

            textScore.text = "Score: " + GirdCandy.Instance.Score;
        });
    }
    public void Retry()
    {
        AudioController.Instance.PlaySoundButtonDown();
        pannelLose.SetActive(false);
        message.SetActive(false);
        menuRetry.SetActive(false);
        AudioController.Instance.PlayAudioMenuGame();
        LevelController.Instance.RetryLevel();
    }

    public void Back()
    {
        AudioController.Instance.PlaySoundButtonDown();
        message.SetActive(false);
        AudioController.Instance.PlayAudioMenuGame();
        menuRetry.transform.DOMove(menuRetry.transform.position + new Vector3(0, 10, 0), 0.5f).OnComplete(() =>
        {
            menuRetry.SetActive(false);
            pannelLose.SetActive(false);
            menuRetry.transform.position = menuRetry.transform.position + new Vector3(0, -10, 0);
        });
    }
}
