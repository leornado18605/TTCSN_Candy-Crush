using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    [SerializeField] public ConfirmUI confirmUI;
    [SerializeField] public LoseUI loseUI;
    [SerializeField] public WinUI winUI;
    [SerializeField] public SettingUI settingUI;
    [SerializeField] public TopUIController topUIController;

}
