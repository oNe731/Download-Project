using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_StartScene : MonoBehaviour
{
    public void Button_Start()
    {
        SceneManager.LoadScene("VisualNovel"); // Intro
    }

    public void Button_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}