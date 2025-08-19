using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Button btnPlay1;
    [SerializeField] private Button btnPlay2;
    [SerializeField] private Button btnPlay3;
    [SerializeField] private Button btnPlay4;

    [SerializeField] private GameObject MainPannel;
    [SerializeField] private LevelData level1;
    [SerializeField] private LevelData level2;
    [SerializeField] private LevelData level3;
    [SerializeField] private LevelData level4;
    [SerializeField] private GirdCandy broad;

    [SerializeField] private GameObject pannelLoading;
    private void Start()
    {
        btnPlay1.onClick.AddListener(delegate
        {
            StartLevel(level1);
        });
        btnPlay2.onClick.AddListener(delegate
        {
            StartLevel(level2);
        });
        btnPlay3.onClick.AddListener(delegate
        {
            StartLevel(level3);
        });
        btnPlay4.onClick.AddListener(delegate
        {
            StartLevel(level4);
        });
    }
    public void StartLevel(LevelData level)
    {
        pannelLoading.SetActive(true);

        DOVirtual.DelayedCall(1f, () =>
        {
            pannelLoading.SetActive(false);
            MainPannel.SetActive(true);
            broad.InitStartGame(level);

        });
    }
}
