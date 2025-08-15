using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Button btnPlay;

    [SerializeField] private GameObject MainPannel;
    [SerializeField] private LevelData levelTest;
    [SerializeField] private GirdCandy broad;

    private void Start()
    {
        btnPlay.onClick.AddListener(delegate
        {
            StartLevel(levelTest);
        });
    }
    public void StartLevel(LevelData level)
    {
        MainPannel.SetActive(true);
        broad.InitStartGame();
    }
}
