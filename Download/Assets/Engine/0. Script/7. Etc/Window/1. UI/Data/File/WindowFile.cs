using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WindowFileData
{
    public WindowManager.FILETYPE fileType;
    public string fileName;

    public List<WindowFileData> childFolders;

    public WindowFileData(WindowManager.FILETYPE type, string name, List<WindowFileData> folders = null)
    {
        fileType = type;
        fileName = name;
        childFolders = folders;
    }
}

public class WindowFile
{
    private static int staticFileIndex = 1;

    private int m_fileIndex;
    private string m_filePath;
    private WindowFileData m_fileData;
    private Action m_action;

    public int FileIndex { get => m_fileIndex; }
    public string FilePath { get => m_filePath; }
    public WindowFileData FileData { get => m_fileData; }
    public Action Action { get => m_action; }

    public WindowFile(string filePath, WindowFileData windowFileData, Action action = null)
    {
        m_fileIndex = staticFileIndex++;

        m_filePath = filePath;
        m_fileData = windowFileData;
        m_action = action;
    }

    public void Set_FileData(WindowFileData newData)
    {
        m_fileData = newData;
    }

    public void Set_FileAction(Action action)
    {
        m_action = action;
    }
}
