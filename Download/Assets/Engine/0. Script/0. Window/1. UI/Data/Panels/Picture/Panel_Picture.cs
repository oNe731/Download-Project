using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Picture : Panel_Popup
{
    private enum TEXTTYPE { TT_FILETYPE, TT_SIZE, TT_DISKSIZE, TT_END }
    private enum BUTTONTYPE { BT_TL, BT_TR, BT_IL, BT_IR, BT_END }
    private enum IMAGETYPE { IT_TOFF, IT_TON, IT_IOFF, IT_ION, IT_END }

    private Transform m_favoriteTransform; // 즐겨찾기 패널 트랜스폼
    private PictureInput m_pictureButton;   // 사진 버튼

    private TMP_InputField m_pathText;               // 폴더 경로
    private Image m_image;                           // 사진 이미지
    private RectTransform m_imageRectTransform;
    private TMP_Text[] m_dataText = new TMP_Text[3]; // 사진 정보
    private Image[] m_pathButtons;

    private Dictionary<int, string> m_filePath = new Dictionary<int, string>(); // 초기 파일 인덱스에 따른 파일 경로
    private Dictionary<string, Sprite> m_pictureSprite = new Dictionary<string, Sprite>(); // 파일 이미지
    private Sprite[] m_pathButtonSprites;

    public Transform FavoriteTransform => m_favoriteTransform;
    public PictureInput PictureButton => m_pictureButton;

    public Panel_Picture() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_PICTURE;

        ResourceManager RM = GameManager.Ins.Resource;
        m_pictureSprite.Add("Wallpaper", RM.Load<Sprite>("1. Graphic/2D/0. Window/WindowWallpaper"));
        //m_pictureSprite.Add("", RM.Load<Sprite>(""));
        //m_pictureSprite.Add("", RM.Load<Sprite>(""));

        m_pathButtonSprites = new Sprite[(int)IMAGETYPE.IT_END];
        m_pathButtonSprites[(int)IMAGETYPE.IT_TOFF] = RM.Load<Sprite>("1. Graphic/2D/0. Window/Window Original form (Document)/Documents Moving Navigator/UI_Window_OriginalForm_Gobutton_OFF");
        m_pathButtonSprites[(int)IMAGETYPE.IT_TON]  = RM.Load<Sprite>("1. Graphic/2D/0. Window/Window Original form (Document)/Documents Moving Navigator/UI_Window_OriginalForm_Gobutton_ON");
        m_pathButtonSprites[(int)IMAGETYPE.IT_IOFF] = RM.Load<Sprite>("1. Graphic/2D/0. Window/Window photo/UI_Window_Photo_NextButtonOFF");
        m_pathButtonSprites[(int)IMAGETYPE.IT_ION]  = RM.Load<Sprite>("1. Graphic/2D/0. Window/Window photo/UI_Window_Photo_NextButtonON");
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            WindowManager WM = GameManager.Ins.Window;
            WindowFile file; 

            string filePath;
            if (m_filePath.TryGetValue(m_activeType, out filePath) == false)
            {
                file = WM.Get_WindowFile(m_activeType);
                if (file == null)
                {
                    Active_Popup(false);
                    return;
                }

                m_filePath.Add(m_activeType, file.FilePath);
            }
            else
                file = WM.Get_WindowFile(m_filePath[m_activeType]);
            if(file == null)
            {
                Active_Popup(false);
                return;
            }

            ImageData imagedata = (ImageData)file.FileData.windowSubData;
            m_pathText.text = file.FilePath;
            m_image.sprite  = m_pictureSprite[imagedata.fileName];

            float fixedHeight = 388f;
            float aspectRatio = m_image.sprite.rect.width / m_image.sprite.rect.height;
            float adjustedWidth = fixedHeight * aspectRatio;
            m_imageRectTransform.sizeDelta = new Vector2(adjustedWidth, fixedHeight);

            m_dataText[(int)TEXTTYPE.TT_FILETYPE].text = "파일 형식 : " + imagedata.fileType;
            m_dataText[(int)TEXTTYPE.TT_SIZE].text     = "크기 : " + imagedata.imageSize;
            m_dataText[(int)TEXTTYPE.TT_DISKSIZE].text = "디스크 할당 크기 : " + imagedata.diskSize;

            Update_Buttons();
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Picture/Panel_Picture", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // 버튼 이벤트 추가
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region 기본 셋팅
        m_childPopup = new List<Panel_Popup>();
        m_childPopup.Add(GameManager.Ins.Window.PictureDelete);

        m_favoriteTransform = m_object.transform.GetChild(2).GetChild(0).GetChild(1);
        m_pictureButton = m_object.transform.GetComponent<PictureInput>();

        m_pathText = m_object.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_InputField>(); // 주소
        m_pathText.enabled = false;
        m_image = m_object.transform.GetChild(3).GetChild(0).GetComponent<Image>();                         // 이미지
        m_imageRectTransform = m_object.transform.GetChild(3).GetChild(0).GetComponent<RectTransform>();
        m_dataText[(int)TEXTTYPE.TT_FILETYPE] = m_object.transform.GetChild(3).GetChild(3).GetChild(0).GetComponent<TMP_Text>(); // 텍스트 정보 파일 형식
        m_dataText[(int)TEXTTYPE.TT_SIZE]     = m_object.transform.GetChild(3).GetChild(3).GetChild(2).GetComponent<TMP_Text>(); // 텍스트 정보 크기
        m_dataText[(int)TEXTTYPE.TT_DISKSIZE] = m_object.transform.GetChild(3).GetChild(3).GetChild(3).GetComponent<TMP_Text>(); // 텍스트 정보 디스크 할당

        m_pathButtons = new Image[(int)BUTTONTYPE.BT_END];
        m_pathButtons[(int)BUTTONTYPE.BT_TL] = m_object.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>();
        m_pathButtons[(int)BUTTONTYPE.BT_TR] = m_object.transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<Image>();
        m_pathButtons[(int)BUTTONTYPE.BT_IL] = m_object.transform.GetChild(3).GetChild(1).GetComponent<Image>();
        m_pathButtons[(int)BUTTONTYPE.BT_IR] = m_object.transform.GetChild(3).GetChild(2).GetComponent<Image>();
        #endregion
    }

    public string Get_Path()
    {
        string filePath;
        m_filePath.TryGetValue(m_activeType, out filePath);

        return filePath;
    }

    #region 삭제
    public void Remove_SelectData() // 선택한 파일 데이터 삭제
    {
        WindowManager WM = GameManager.Ins.Window;
        WindowFile file = WM.Get_WindowFile(WM.Picture.Get_Path());
        if (file == null)
            return;

        string directoryPath = Path.GetDirectoryName(file.FilePath);
        // 경로에 따른 삭제
        if (directoryPath == WM.BackgroundPath) // 바탕화면일 시
        {
            // 바탕화면 아이콘 삭제
            WM.FileIconSlots.Remove_FileIcon(file.FilePath);
        }
        else // 바탕화면이 아닐 시
        {
            // 현재 경로인 부모 폴더 자식 리스트에서 삭제
            WindowFile parentfile = WM.Get_WindowFile(directoryPath);
            parentfile.Remove_ChildFile(file.FileData);
        }

        // 딕셔너리에서 해당 키값 삭제
        WM.FileData.Remove(file.FilePath);

        // 휴지통으로 경로 변경 후 재추가
        string trashbinPath = WM.BackgroundPath + "\\" + "휴지통";
        WindowFile newPathFile = WM.Get_WindowFile(WM.Get_FullFilePath(trashbinPath, file.FileData.fileName), file.FileData);
        newPathFile.Set_PrevfilePath(file.FilePath);

        // 휴지통 자식으로 등록
        WindowFile trashbinFile = WM.Get_WindowFile(trashbinPath);
        trashbinFile.Add_ChildFile(file.FileData);

        // (보류) 앞으로가기를 할 수 있으면 해당 이미지로 변경
        // 없으면 비활성화
        Active_Popup(false);
    }
    #endregion

    #region 뒤로가기/ 앞으로가기
    public void Folder_Back() // 뒤로가기
    {
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        var fileList = filedatas.OrderBy(f => f.Key).ToList();
        string currentPath = Get_Path();
        string commonFolderPath = GameManager.Ins.Window.Get_ParentPath(currentPath);
        int currentIndex = fileList.FindIndex(f => f.Value.FilePath == currentPath);

        if (currentIndex > 0) // 첫 번째 파일이 아닌 경우에만 탐색
        {
            // 현재 파일 이전 파일부터 역순으로 탐색하여 동일 폴더 내의 이미지 파일을 찾음
            for (int i = currentIndex - 1; i >= 0; i--)
            {
                string targetParentPath = GameManager.Ins.Window.Get_ParentPath(fileList[i].Value.FilePath);
                if (targetParentPath == commonFolderPath && fileList[i].Value.FileData.fileType == WindowManager.FILETYPE.TYPE_PICTURE)
                {
                    Active_Popup(true, fileList[i].Value.FileIndex);
                    break;
                }
            }
        }

        Update_Buttons();
    }

    public void Folder_Again() // 앞으로가기
    {
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        var fileList = filedatas.OrderBy(f => f.Key).ToList();
        string currentPath = Get_Path();
        string commonFolderPath = GameManager.Ins.Window.Get_ParentPath(currentPath);
        int currentIndex = fileList.FindIndex(f => f.Value.FilePath == currentPath);

        if (currentIndex >= 0 && currentIndex < fileList.Count - 1) // 마지막 파일이 아닌 경우에만 탐색
        {
            // 현재 파일 이후 파일부터 순차적으로 탐색하여 동일 폴더 내의 이미지 파일을 찾음
            for (int i = currentIndex + 1; i < fileList.Count; i++)
            {
                string targetParentPath = GameManager.Ins.Window.Get_ParentPath(fileList[i].Value.FilePath);
                if (targetParentPath == commonFolderPath && fileList[i].Value.FileData.fileType == WindowManager.FILETYPE.TYPE_PICTURE)
                {
                    Active_Popup(true, fileList[i].Value.FileIndex);
                    break;
                }
            }
        }

        Update_Buttons();
    }

    public void Update_Buttons()
    {
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        var fileList = filedatas.OrderBy(f => f.Key).ToList();
        string currentPath = Get_Path();
        string commonFolderPath = GameManager.Ins.Window.Get_ParentPath(currentPath);
        int currentIndex = fileList.FindIndex(f => f.Value.FilePath == currentPath);

        bool canGoBack = false;
        bool canGoForward = false;

        // 뒤로 가기 가능 여부 확인
        if (currentIndex > 0)
        {
            for (int i = currentIndex - 1; i >= 0; i--)
            {
                string targetParentPath = GameManager.Ins.Window.Get_ParentPath(fileList[i].Value.FilePath);
                if (targetParentPath == commonFolderPath && fileList[i].Value.FileData.fileType == WindowManager.FILETYPE.TYPE_PICTURE)
                {
                    canGoBack = true;
                    break;
                }
            }
        }

        // 앞으로 가기 가능 여부 확인
        if (currentIndex >= 0 && currentIndex < fileList.Count - 1)
        {
            for (int i = currentIndex + 1; i < fileList.Count; i++)
            {
                string targetParentPath = GameManager.Ins.Window.Get_ParentPath(fileList[i].Value.FilePath);
                if (targetParentPath == commonFolderPath && fileList[i].Value.FileData.fileType == WindowManager.FILETYPE.TYPE_PICTURE)
                {
                    canGoForward = true;
                    break;
                }
            }
        }

        // 버튼 이미지 업데이트
        m_pathButtons[(int)BUTTONTYPE.BT_TL].sprite = canGoBack ? m_pathButtonSprites[(int)IMAGETYPE.IT_TON] : m_pathButtonSprites[(int)IMAGETYPE.IT_TOFF];
        m_pathButtons[(int)BUTTONTYPE.BT_TR].sprite = canGoForward ? m_pathButtonSprites[(int)IMAGETYPE.IT_TON] : m_pathButtonSprites[(int)IMAGETYPE.IT_TOFF];
        m_pathButtons[(int)BUTTONTYPE.BT_IL].sprite = canGoBack ? m_pathButtonSprites[(int)IMAGETYPE.IT_ION] : m_pathButtonSprites[(int)IMAGETYPE.IT_IOFF];
        m_pathButtons[(int)BUTTONTYPE.BT_IR].sprite = canGoForward ? m_pathButtonSprites[(int)IMAGETYPE.IT_ION] : m_pathButtonSprites[(int)IMAGETYPE.IT_IOFF];
    }
    #endregion
}
