using UnityEngine;

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
        GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => GameManager.Instance.Change_Scene("VisualNovel"), 0f, false);
    }

    public void Button_Western()
    {
        GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => GameManager.Instance.Change_Scene("Western"), 0f, false);
    }

    public void Button_Horror()
    {
        GameManager.Instance.UI.Start_FadeOut(1f, Color.black, () => GameManager.Instance.Change_Scene("Horror"), 0f, false);
    }
}
