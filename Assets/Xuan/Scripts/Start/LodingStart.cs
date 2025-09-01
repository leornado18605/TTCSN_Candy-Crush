using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodingStart : MonoBehaviour
{
    [SerializeField] private GameObject pannelLoading1;
    [SerializeField] private GameObject pannelLoading2;
    [SerializeField] private GameObject pannelStart;

    private void Start()
    {    
        DOVirtual.DelayedCall(4f, () =>
        {
            pannelLoading1.SetActive(false);
            pannelLoading2.SetActive(true);

            DOVirtual.DelayedCall(1.5f, () =>
            {
                pannelLoading2.SetActive(false);
                pannelStart.SetActive(true);
            });
        });
    }
}
