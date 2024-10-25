using System;
using System.Collections.Generic;

[Serializable]
public class FoldersData
{
    public List<Folder> childFolders;
}

public struct Folder
{
    public WindowManager.FILETYPE fileType;
    public string fileName;
    public List<Folder> childFolders;
}
