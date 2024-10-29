using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileInput : MonoBehaviour
{
    private TMP_InputField m_nameField;
    private TMP_InputField m_pathField;

    public TMP_InputField NameField => m_nameField;
    public TMP_InputField PathField => m_pathField;

    public void Start_FileInput()
    {
        m_nameField = transform.GetChild(0).GetChild(1).GetComponent<TMP_InputField>();
        m_pathField = transform.GetChild(1).GetChild(1).GetComponent<TMP_InputField>();
    }

    public void Button_Save()
    {
        if (m_nameField.text == "")
            return;

        WindowManager WI = GameManager.Ins.Window;

        // 파일 추가
        if(WI.FileIconSlots.Add_NewFileIcon(WindowManager.FILETYPE.TYPE_TXT, m_nameField.text) == true)
        {
            WindowFile file = WI.Get_WindowFile(WI.Get_FullFilePath(WI.BackgroundPath, m_nameField.text), new WindowFileData(WindowManager.FILETYPE.TYPE_TXT, m_nameField.text));
            file.Set_FileAction(() => GameManager.Ins.Window.MEMO.Active_Popup(true, file.FileIndex));

            WI.MEMO.Memos.Add(file.FileIndex, WI.MEMO.InputField.text);
            m_nameField.text = "";
        }
    }

    public void Button_Cancel()
    {
        GameManager.Ins.Window.FOLDER.Object.transform.GetChild(4).gameObject.SetActive(false);
    }
}
