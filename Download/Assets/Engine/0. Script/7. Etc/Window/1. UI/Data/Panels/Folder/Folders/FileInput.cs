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

        WindowManager WM = GameManager.Ins.Window;
        string fileName = WM.Get_FileName(WM.BackgroundPath, m_nameField.text);

        // 파일 추가
        if (WM.FileIconSlots.Add_FileIcon(WindowManager.FILETYPE.TYPE_TXT, fileName) == true)
        {
            WindowFile file = WM.Get_WindowFile(WM.Get_FullFilePath(WM.BackgroundPath, fileName), new WindowFileData(WindowManager.FILETYPE.TYPE_TXT, fileName));
            file.Set_FileAction(() => GameManager.Ins.Window.Memo.Active_Popup(true, file.FileIndex));

            WM.Memo.Memos.Add(file.FileIndex, WM.Memo.InputField.text);
            m_nameField.text = "";
        }
    }

    public void Button_Cancel()
    {
        GameManager.Ins.Window.Folder.Object.transform.GetChild(4).gameObject.SetActive(false);
    }
}
