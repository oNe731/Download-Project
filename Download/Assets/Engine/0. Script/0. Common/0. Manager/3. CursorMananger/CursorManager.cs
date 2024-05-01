using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CURSORTYPE { CT_ORIGIN, CT_BASIC, CT_NOVELSHOOT, CT_END };

public class CursorManager : MonoBehaviour
{
    private static CursorManager m_instance = null;
    public static CursorManager Instance
    {
        get
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    [Header("Resource")]
    [SerializeField] Texture2D m_shootGameCursor;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Change_Cursor(CURSORTYPE type)
    {
        Texture2D texture = null;
        Vector2 hotspot = Vector2.zero;
        CursorMode mode = CursorMode.ForceSoftware;

        switch (type)
        {
            case CURSORTYPE.CT_ORIGIN:
                texture = null;
                hotspot = Vector2.zero;
                mode = CursorMode.Auto;
                break;

            case CURSORTYPE.CT_BASIC:
                break;

            case CURSORTYPE.CT_NOVELSHOOT:
                texture = m_shootGameCursor;
                hotspot = new Vector2(texture.width / 2, texture.height / 2);
                mode = CursorMode.ForceSoftware;
                break;
        }

        Cursor.SetCursor(texture, hotspot, mode);
    }
}
