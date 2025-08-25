using DG.Tweening;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : Singleton<StartGame>
{
    [SerializeField] private Button btnPlay1;
    [SerializeField] private Button btnPlay2;
    [SerializeField] private Button btnPlay3;
    [SerializeField] private Button btnPlay4;

    [SerializeField] private Button btnPlay;


    [SerializeField] private GameObject MainPannel;
    [SerializeField] private LevelData level1;
    [SerializeField] private LevelData level2;
    [SerializeField] private LevelData level3;
    [SerializeField] private LevelData level4;
    private int currentLevelIndex = 1;

    private LevelData currentLevel;

    [SerializeField] private GameObject pannelLoading;
    [SerializeField] private ConfirmUI confirmPlay;
    private void Start()
    {
        AudioController.Instance.PlayAudioMenuGame();
        btnPlay1.onClick.AddListener(delegate
        {
            confirmPlay.SetOn(true);
            currentLevelIndex = 1;
            confirmPlay.SetText(1);
            currentLevel = level1;
            confirmPlay.SettextInMenu(level1.moveLimit, level1.targetScore);
        });
        btnPlay2.onClick.AddListener(delegate
        {
            confirmPlay.SetOn(true);
            currentLevelIndex = 2;
            confirmPlay.SetText(2);
            currentLevel = level2;
            confirmPlay.SettextInMenu(level2.moveLimit, level2.targetScore);
        });
        btnPlay3.onClick.AddListener(delegate
        {
            confirmPlay.SetOn(true);
            currentLevelIndex = 3;
            confirmPlay.SetText(3);
            currentLevel = level3;
            confirmPlay.SettextInMenu(level3.moveLimit, level3.targetScore);
        });
        btnPlay4.onClick.AddListener(delegate
        {
            confirmPlay.SetOn(true);
            currentLevelIndex = 4;
            confirmPlay.SetText(4);
            currentLevel = level4;
            confirmPlay.SettextInMenu(level4.moveLimit, level4.targetScore);
        });
        btnPlay.onClick.AddListener(delegate
        {
            if (currentLevel != null)
            {
                StartLevel(currentLevel);
            }
        });
    }
    public void StartLevel(LevelData level)
    {
        pannelLoading.SetActive(true);
        confirmPlay.SetOnFalse();

        DOVirtual.DelayedCall(1f, () =>
        {
            pannelLoading.SetActive(false);
            MainPannel.SetActive(true);
            GirdCandy.Instance.InitStartGame(level);
        });
    }
    public void RetryLevel()
    {
        confirmPlay.SetOn(true);
    }
    public void NextLevel()
    {
        currentLevelIndex++;
        if(currentLevelIndex > 4)
        {
            currentLevelIndex = 1;
        }
        confirmPlay.SetOn(true);
        confirmPlay.SetText(currentLevelIndex);
        currentLevel = GetLevel();
        confirmPlay.SettextInMenu(currentLevel.moveLimit, currentLevel.targetScore);
    }
    public LevelData GetLevel()
    {
        if(currentLevelIndex == 1)
        {
            return level1;
        }
        else if(currentLevelIndex == 2)
        {
            return level2;
        }
        else if(currentLevelIndex == 3)
        {
            return level3;
        }
        else if(currentLevelIndex == 4)
        {
            return level4;
        }
        return null;
    }
}
