using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : Singleton<LevelController>
{
    [SerializeField] private List<Button> btnList;
    [SerializeField] private Button btnPlay;
    [SerializeField] private List<LevelData> levelDatas;
    private LevelData currentLevel;
    private int currentLevelIndex = 1;
    [SerializeField] private GameObject MainPannel;
    [SerializeField] private GameObject pannelLoading;
    [SerializeField] private ConfirmUI confirmPlay;
    private int unlockedLevel;
    private void Start()
    {
        AudioController.Instance.PlayAudioMenuGame();
        unlockedLevel = PlayerPrefs.GetInt("LevelUnlocked", 1);

        for (int i = 0; i < btnList.Count; i++)
        {
            int level = i + 1;

            if (level <= unlockedLevel)
            {
                btnList[i].interactable = true; // cho bấm
            }
            else
            {
                btnList[i].interactable = false; // khóa lại
            }

            btnList[i].onClick.AddListener(() => LoadLevel(level));
        }

        btnPlay.onClick.AddListener(delegate
        {
            AudioController.Instance.PlaySoundButtonDown();
            if (currentLevel != null)
            {
                StartLevel(currentLevel);
            }
        });
    }

    public void LoadLevel(int level)
    {
        AudioController.Instance.PlaySoundButtonDown();
        currentLevelIndex = level;
        confirmPlay.SetOn(true);
        confirmPlay.SetText(level);
        confirmPlay.SettextInMenu(levelDatas[level-1].moveLimit, levelDatas[level-1].targetScore);
        currentLevel = levelDatas[level - 1];
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
        UnlockNextLevel(currentLevelIndex - 1);
        if (currentLevelIndex > levelDatas.Count)
        {
            currentLevelIndex = levelDatas.Count;
        }
        confirmPlay.SetOn(true);
        confirmPlay.SetText(currentLevelIndex);
        currentLevel = levelDatas[currentLevelIndex - 1];
        confirmPlay.SettextInMenu(currentLevel.moveLimit, currentLevel.targetScore);
    }
    public void UnlockNextLevel(int currentLevel)
    {
        int nextLevel = currentLevel + 1;

        if (nextLevel > PlayerPrefs.GetInt("LevelUnlocked", 1))
        {
            btnList[nextLevel - 1].interactable = true;
            PlayerPrefs.SetInt("LevelUnlocked", nextLevel);
            PlayerPrefs.Save();
        }
    }
}
