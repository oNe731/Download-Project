using UnityEngine;

public class StartManager : MonoBehaviour
{
    public void Button_Start()
    {
        GameManager.Ins.Change_Scene("Window"); // Intro
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