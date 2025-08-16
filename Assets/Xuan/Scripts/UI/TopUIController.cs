using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextID
{
    ChangeScore,
    ChangeMoveLimit,
}
public class TopUIController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textMoveLimit;

    private void OnEnable()
    {
        ObserverManager<TextID>.AddDesgisterEvent(TextID.ChangeScore, SetTextScore);
        ObserverManager<TextID>.AddDesgisterEvent(TextID.ChangeMoveLimit, SetTextMoveLimit);
    }
    private void OnDisable()
    {
        ObserverManager<TextID>.RemoveAddListener(TextID.ChangeScore, SetTextScore);
        ObserverManager<TextID>.RemoveAddListener(TextID.ChangeMoveLimit, SetTextMoveLimit);
    }

    public void SetTextScore(object obj)
    {
        textScore.text = obj.ToString();
    }

    public void SetTextMoveLimit(object obj)
    {
        textMoveLimit.text = obj.ToString();
    }
}
