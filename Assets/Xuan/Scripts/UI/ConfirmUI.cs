using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTile;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private TextMeshProUGUI textRetry;
    [SerializeField] private TextMeshProUGUI textMove;
    [SerializeField] private TextMeshProUGUI textScore;

    [SerializeField] private Button btnBack;
    [SerializeField] private GameObject pannel;
    [SerializeField] private GameObject menu;

    private bool isMove = false;

    private void Start()
    {
        btnBack.onClick.AddListener(delegate
        {
            AudioController.Instance.PlaySoundButtonDown();
            SetOn(false);
        });
    }

    public void SetOn(bool val)
    {
        if (isMove) return;

        isMove = true;
        if (val)
        {
            pannel.SetActive(val);
            menu.SetActive(val);
            Vector3 taggetPos = menu.transform.position;

            menu.transform.position = menu.transform.position + new Vector3(0, 10, 0);
            menu.transform.DOMove(taggetPos, 0.5f).OnComplete(()=>
            {
                isMove = false;
            });
        }
        else
        {
            Vector3 taggetPos = menu.transform.position;
            menu.transform.DOMove(taggetPos + new Vector3(0, 10, 0), 0.5f).OnComplete(() =>
            {
                menu.gameObject.SetActive(val);
                pannel.SetActive(val);
                menu.transform.position = taggetPos;
                isMove = false;
            });
        }
    }
    public void SetOnFalse()
    {
        pannel.SetActive(false);
        menu.SetActive(false);  
    }

    public void SetText(int indexLevel)
    {
        textTile.text = "Level " + indexLevel.ToString();
        textLevel.text = "Level " + indexLevel.ToString();
        textRetry.text = "Level " + indexLevel.ToString();
    }
    public void SettextInMenu(int move, int score)
    {
        textMove.text = "Move Limits : " + move.ToString();
        textScore.text = "Score Goals : " + score.ToString();
    }
}
