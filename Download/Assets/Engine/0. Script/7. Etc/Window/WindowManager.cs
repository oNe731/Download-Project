using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowManager : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void Button_VisualNovel()
    {
        SceneManager.LoadScene("VisualNovel");
    }

    public void Button_Western()
    {
        SceneManager.LoadScene("Western");
    }
}
