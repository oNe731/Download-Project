using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.UI.Start_FadeIn(1f, Color.black);
    }

    private void Update()
    {
        
    }

    public void Button_VisualNovel()
    {
        GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => SceneManager.LoadScene("VisualNovel"), 0f, false);
    }

    public void Button_Western()
    {
        GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => SceneManager.LoadScene("Western"), 0f, false);
    }

    public void Button_Horror()
    {
        GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => SceneManager.LoadScene("Horror"), 0f, false);
    }
}
