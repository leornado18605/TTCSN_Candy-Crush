using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private Button btn;
    public Button Btn => btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    public void WinGame()
    {
        btn.image.sprite = ButtonImageMap.Instance.imageWin;
    }
}
