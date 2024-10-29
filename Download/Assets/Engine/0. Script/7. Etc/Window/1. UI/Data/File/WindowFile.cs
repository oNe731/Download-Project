using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#region 윈도우 파일 데이터
public struct WindowFileData
{
    public WindowManager.FILETYPE fileType;
    public string fileName;

    [JsonConverter(typeof(WindowFileDataSubDataConverter))]
    public WindowFileDataSubData windowSubData;

    public WindowFileData(WindowManager.FILETYPE type, string name, WindowFileDataSubData subData = null)
    {
        fileType = type;
        fileName = name;
        windowSubData = subData;
    }
}

public class WindowFileDataSubDataConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(WindowFileDataSubData).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        if (jsonObject["childFolders"] != null)
        {
            return jsonObject.ToObject<FolderData>();
        }
        else
        {
            return jsonObject.ToObject<None>();
        }

        throw new JsonSerializationException("Unknown DialogSubDataType");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
    }
}


#region 윈도우 세부 파일 데이터
public interface WindowFileDataSubData
{
}

[Serializable]
public struct None : WindowFileDataSubData
{
}

[Serializable]
public struct FolderData : WindowFileDataSubData
{
    public List<WindowFileData> childFolders;
}
#endregion
#endregion

public class WindowFile
{
    private static int staticFileIndex = 1;

    private int m_fileIndex;   // 파일 고유 인덱스
    private string m_filePath; // 파일 경로

    private WindowFileData m_fileData; // 파일 데이터
    private Action m_action;           // 파일 액션

    private bool m_favorite = false; // 즐겨찾기 여부

    #region 프로퍼티
    public int FileIndex { get => m_fileIndex; }
    public string FilePath { get => m_filePath; }
    public WindowFileData FileData { get => m_fileData; }
    public Action Action { get => m_action; }
    public bool Favorite { get => m_favorite; set => m_favorite = value; }
    #endregion

    public WindowFile(string filePath, WindowFileData windowFileData, Action action)
    {
        m_fileIndex = staticFileIndex++;

        m_filePath = filePath;
        m_fileData = windowFileData;
        m_action = action;
    }

    public void Set_FileData(WindowFileData data)
    {
        m_fileData = data;
    }

    public void Set_FileAction(Action action)
    {
        m_action = action;
    }

    public void Add_ChildFile(WindowFileData windowFileData) // 자식 파일 추가
    {
        if (m_fileData.fileType != WindowManager.FILETYPE.TYPE_FOLDER)
            return;

        FolderData folderData = (FolderData)m_fileData.windowSubData;
        folderData.childFolders.Add(windowFileData);
    }
}
