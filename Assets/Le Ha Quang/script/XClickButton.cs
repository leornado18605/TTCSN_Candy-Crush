using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class XClickButton : MonoBehaviour
{
    public void OnXClick()
    {
        SceneManager.UnloadSceneAsync("Setting scene");
    }
}
