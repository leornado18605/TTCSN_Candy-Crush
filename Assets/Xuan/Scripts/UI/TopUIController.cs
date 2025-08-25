using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextID
{
    ChangeScore,
    SetupScore,
    ChangeMoveLimit,
}
public class TopUIController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textMoveLimit;
    [SerializeField] private ScoreUI scoreUI;

    private void OnEnable()
    {
        ObserverManager<TextID>.AddDesgisterEvent(TextID.ChangeScore, SetScore);
        ObserverManager<TextID>.AddDesgisterEvent(TextID.SetupScore, SetupScore);
        ObserverManager<TextID>.AddDesgisterEvent(TextID.ChangeMoveLimit, SetTextMoveLimit);
    }
    private void OnDisable()
    {
        ObserverManager<TextID>.RemoveAddListener(TextID.ChangeScore, SetScore);
        ObserverManager<TextID>.RemoveAddListener(TextID.SetupScore, SetupScore);
        ObserverManager<TextID>.RemoveAddListener(TextID.ChangeMoveLimit, SetTextMoveLimit);
    }

    public void SetTextMoveLimit(object obj)
    {
        textMoveLimit.text = obj.ToString();
    }

    public void SetScore(object obj)
    {
        if(obj == null) return;
        int score = (int)obj;

        if(scoreUI != null)
        {
            scoreUI.UpdateScoreUI(score);
        }
    }

    public void SetupScore(object obj)
    {
        if (obj == null) return;
        int score = (int)obj;
        if (scoreUI != null)
        {
            scoreUI.SetScore(score);
        }
    }
}
