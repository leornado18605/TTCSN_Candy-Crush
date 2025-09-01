using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : MonoBehaviour
{
    [SerializeField] private Image loadingIcon;

    private void Update()
    {
        loadingIcon.transform.Rotate(0f, 0f, -50f * Time.deltaTime);
    }
}
