using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginInput : MonoBehaviour
{
    private TMP_InputField m_nameInput;

    private void Start()
    {
        m_nameInput = transform.GetChild(2).GetComponent<TMP_InputField>();
    }

    public void Button_InputName()
    {
        GameManager.Ins.PlayerName = m_nameInput.text;
        GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW);
    }

    public void Button_Exit()
    {
        GameManager.Ins.End_Game();
    }
}
