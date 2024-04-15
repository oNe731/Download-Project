using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private float m_deltaTime = 0.0f;

    private void Awake()
    {
        Application.targetFrameRate = 144;
    }

    private void Update()
    {
#if UNITY_EDITOR
        m_deltaTime += (Time.unscaledDeltaTime - m_deltaTime) * 0.1f;
#endif
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        int Width = Screen.width;
        int Height = Screen.height;
        Rect RectSize = new Rect(10, 10, Width, Height);

        GUIStyle Style = new GUIStyle();
        Style.alignment = TextAnchor.UpperLeft;
        Style.fontSize = 20;
        Style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        //float Msec = DeltaTime * 1000.0f;
        float Fps = 1.0f / m_deltaTime;
        //string Text = string.Format("{0:0.0} ms ({1:0.} fps)", Msec, Fps);
        string Text = string.Format("{0:0.} FPS", Fps);

        GUI.Label(RectSize, Text, Style);
#endif
    }
}
