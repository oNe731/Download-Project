using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#region 윈도우 파일 데이터
public struct WindowFileData
{
    public WindowManager.FILETYPE fileType;
    public string fileName;
    public Action fileAction;
    public string fileprevfilePath; // 이전 파일 경로

    [JsonConverter(typeof(WindowFileDataSubDataConverter))]
    public WindowFileDataSubData windowSubData;

    public WindowFileData(WindowManager.FILETYPE type, string name, Action action = null, WindowFileDataSubData subData = null, string prevfilePath = "")
    {
        fileType = type;
        fileName = name;
        fileAction = action;
        windowSubData = subData;
        fileprevfilePath = prevfilePath;
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
        else if (jsonObject["imageSize"] != null)
        {
            return jsonObject.ToObject<ImageData>();
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

[Serializable]
public struct ImageData : WindowFileDataSubData // 사용하는 사진 실제 윈도우 속성 값
{
    public string fileName;
    public string fileType;
    public string imageSize;
    public string diskSize;
}
#endregion
#endregion

public class WindowFile
{
    private static int staticFileIndex = 100;

    private int m_fileIndex;   // 파일 고유 인덱스
    private string m_filePath; // 파일 경로
    private bool m_favorite = false; // 즐겨찾기 여부


    private WindowFileData m_fileData; // 파일 데이터

    #region 프로퍼티
    public int FileIndex { get => m_fileIndex; }
    public string FilePath { get => m_filePath; }
    public WindowFileData FileData { get => m_fileData; }
    public bool Favorite { get => m_favorite; set => m_favorite = value; }
    #endregion

    public WindowFile(string filePath, WindowFileData windowFileData)
    {
        m_fileIndex = staticFileIndex++;

        m_filePath = filePath;
        m_fileData = windowFileData;
    }

    public void Set_FileData(WindowFileData data)
    {
        m_fileData = data;
    }

    public void Set_FileAction(Action action)
    {
        m_fileData.fileAction = action;
    }

    public void Add_ChildFile(WindowFileData windowFileData) // 자식 파일 추가
    {
        if (m_fileData.fileType != WindowManager.FILETYPE.TYPE_FOLDER && m_fileData.fileType != WindowManager.FILETYPE.TYPE_TRASHBIN)
            return;

        FolderData folderData = (FolderData)m_fileData.windowSubData;
        folderData.childFolders.Add(windowFileData);
    }

    public void Remove_ChildFile(WindowFileData windowFileData)
    {
        if (m_fileData.fileType != WindowManager.FILETYPE.TYPE_FOLDER && m_fileData.fileType != WindowManager.FILETYPE.TYPE_TRASHBIN)
            return;

        FolderData folderData = (FolderData)m_fileData.windowSubData;
        int index = folderData.childFolders.FindIndex(child => child.fileName == windowFileData.fileName);
        if (index != -1)
        {
            folderData.childFolders.RemoveAt(index);
        }
    }

    public void Set_PrevfilePath(string prevfilePath)
    {
        m_fileData.fileprevfilePath = prevfilePath;
    }
}
