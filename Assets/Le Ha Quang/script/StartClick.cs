using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startClick : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Map scene");
    }
}
