using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Folder : Panel_Popup
{
    public enum TYPE { TYPE_NONE, TYPE_GAMEICON, TYPE_END }
    public enum EVENT { EVENT_GAMEICON, EVENT_END }

    private List<FoldersData> m_foldersData;
    private List<bool> m_eventBool;

    private Transform m_folderTransform;
    public Transform FolderTransform => m_folderTransform;

    public Panel_Folder() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_FOLDER;

        m_eventBool = new List<bool>();
        for (int i = 0; i < (int)EVENT.EVENT_END; ++i)
            m_eventBool.Add(false);
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_GAMEICON:
                    if (m_eventBool[(int)EVENT.EVENT_GAMEICON] == true)
                        return;
                    Set_FolderData(GameManager.Ins.Load_JsonData<FoldersData>("4. Data/0. Window/Folders/Folders_Games"));
                    Start_Event(EVENT.EVENT_GAMEICON);
                    break;
            }
        }
        else
        {
            switch (m_activeType)
            {
                case (int)TYPE.TYPE_GAMEICON:
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(1, 1, WindowManager.FILETYPE.TYPE_NOVEL, "오싹오싹 밴드부", () => GameManager.Ins.Window.WindowButton.Button_VisualNovel());
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(2, 1, WindowManager.FILETYPE.TYPE_WESTERN, "THE LEGEND COWBOY", () => GameManager.Ins.Window.WindowButton.Button_Western());
                    GameManager.Ins.Window.FileIconSlots.Add_FileIcon(3, 1, WindowManager.FILETYPE.TYPE_HORROR, "THE HOSPITAL", () => GameManager.Ins.Window.WindowButton.Button_Horror());
                    break;
            }
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Panels/Panel_Folder", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_folderTransform = m_object.transform.GetChild(3).GetChild(0).GetChild(0);

        if (m_foldersData != null)
            Set_FolderData(m_foldersData);
        #endregion
    }

    public override void Update_Data()
    {
    }

    public override void Unload_Scene()
    {
    }

    public void Set_FolderData(List<FoldersData> foldersDatas) // 창 열기 전 정보 셋팅
    {
        // 자식 삭제
        int childCount = m_folderTransform.childCount - 1;
        for (int i = 1; i < childCount; i++)
            GameManager.Ins.Resource.Destroy(m_folderTransform.GetChild(i).gameObject);

        // 패널 생성
        m_foldersData = foldersDatas;
        int count = m_foldersData[0].childFolders.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Folder/Folder_FileList", m_folderTransform);
            if (obj != null)
            {
                obj.transform.SetSiblingIndex(m_folderTransform.childCount - 2);
                FolderBox folderbox = obj.GetComponent<FolderBox>();
                if(folderbox != null)
                    folderbox.Set_FolderBox(m_foldersData[0].childFolders[i]);
            }
        }
    }

    public void Start_Event(EVENT type)
    {
        if (m_eventBool[(int)type] == true)
            return;

        m_eventBool[(int)type] = true;
        switch (type)
        {
            case EVENT.EVENT_GAMEICON:
                GameManager.Ins.StartCoroutine(Destroy_GameIcon());
                break;
        }
    }

    private IEnumerator Destroy_GameIcon()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            int currentIndex = 0;
            int childCount = m_folderTransform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                GameObject obj = m_folderTransform.GetChild(currentIndex).gameObject;
                FolderBox folderBox = obj.GetComponent<FolderBox>();
                if(folderBox != null)
                {
                    int fileIndex = (int)folderBox.FolderData.fileType;
                    if (fileIndex >= (int)WindowManager.FILETYPE.TYPE_BLACKOUT && fileIndex <= (int)WindowManager.FILETYPE.TYPE_THET || fileIndex == 18)
                    {
                        // 디스트로이 박스 활성화
                        folderBox.Set_ClickImage(FolderBox.BOXIMAGE.BT_DESTROY);
                        yield return new WaitForSeconds(0.5f);

                        // 빈칸으로 변경
                        folderBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        folderBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                        yield return new WaitForSeconds(0.25f);

                        // 삭제
                        m_foldersData[0].childFolders.RemoveAt(currentIndex - 1);
                        GameManager.Ins.Resource.Destroy(folderBox.gameObject);
                        yield return new WaitForSeconds(0.3f);
                    }
                    else
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    currentIndex++;
                }
            }
            break;
        }
    }
}
