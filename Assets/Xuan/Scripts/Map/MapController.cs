using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private Button btnSetting;
    [SerializeField] private GameObject settingPanel;

    private void Start()
    {
        btnSetting.onClick.AddListener(() =>
        {
            settingPanel.SetActive(true);

        });
    }
}
