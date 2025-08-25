using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Image scoreBar;
    [SerializeField] private GameObject star1;
    [SerializeField] private GameObject star2;
    [SerializeField] private GameObject star3;
    private int score;
    private int currentScore;
    public void SetScore(int s)
    {
        currentScore = 0;
        score = s;
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        scoreBar.fillAmount = 0;
    }

    public void UpdateScoreUI(int s)
    {
        currentScore = s;
        float cal = (float)currentScore / score;

        StartCoroutine(SetFillScore(cal));
    }

    IEnumerator SetFillScore(float targetFill)
    {
        while (scoreBar.fillAmount < targetFill)
        {
            scoreBar.fillAmount += Time.deltaTime;
            yield return null;
        }
        scoreBar.fillAmount = targetFill;
        CheckStar();
    }

    public void CheckStar()
    {
        if (scoreBar.fillAmount >= 0.33f)
        {
            star1.SetActive(true);
        }
        if (scoreBar.fillAmount >= 0.66f)
        {
            star2.SetActive(true);
        }
        if (scoreBar.fillAmount >= 1f)
        {
            star3.SetActive(true);
        }
    }
}
