using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowManager : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.Start_FadeIn(1f, Color.black);
    }

    private void Update()
    {
        
    }

    public void Button_VisualNovel()
    {
        UIManager.Instance.Start_FadeOut(1f, Color.black, () => SceneManager.LoadScene("VisualNovel"), 0f, false);
    }

    public void Button_Western()
    {
        UIManager.Instance.Start_FadeOut(1f, Color.black, () => SceneManager.LoadScene("Western"), 0f, false);
    }
}
