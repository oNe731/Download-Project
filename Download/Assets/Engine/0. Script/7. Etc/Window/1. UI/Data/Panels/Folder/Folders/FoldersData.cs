using System;
using System.Collections.Generic;

[Serializable]
public class FoldersData
{
    public List<WindowFileData> childFolders;

    public FoldersData(List<WindowFileData> windowFileDatas)
    {
        childFolders = windowFileDatas;
    }
}
