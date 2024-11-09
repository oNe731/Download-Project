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

    private Transform m_favoriteTransform; // ���ã�� �г� Ʈ������
    private PictureInput m_pictureButton;   // ���� ��ư

    private TMP_InputField m_pathText;               // ���� ���
    private Image m_image;                           // ���� �̹���
    private RectTransform m_imageRectTransform;
    private TMP_Text[] m_dataText = new TMP_Text[3]; // ���� ����
    private Image[] m_pathButtons;

    private Dictionary<int, string> m_filePath = new Dictionary<int, string>(); // �ʱ� ���� �ε����� ���� ���� ���
    private Dictionary<string, Sprite> m_pictureSprite = new Dictionary<string, Sprite>(); // ���� �̹���
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

            m_dataText[(int)TEXTTYPE.TT_FILETYPE].text = "���� ���� : " + imagedata.fileType;
            m_dataText[(int)TEXTTYPE.TT_SIZE].text     = "ũ�� : " + imagedata.imageSize;
            m_dataText[(int)TEXTTYPE.TT_DISKSIZE].text = "��ũ �Ҵ� ũ�� : " + imagedata.diskSize;

            Update_Buttons();
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Picture/Panel_Picture", canvas.GetChild(3));
        m_object.SetActive(m_select);

        // ��ư �̺�Ʈ �߰�
        m_object.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Putdown_Popup());
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region �⺻ ����
        m_childPopup = new List<Panel_Popup>();
        m_childPopup.Add(GameManager.Ins.Window.PictureDelete);

        m_favoriteTransform = m_object.transform.GetChild(2).GetChild(0).GetChild(1);
        m_pictureButton = m_object.transform.GetComponent<PictureInput>();

        m_pathText = m_object.transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<TMP_InputField>(); // �ּ�
        m_pathText.enabled = false;
        m_image = m_object.transform.GetChild(3).GetChild(0).GetComponent<Image>();                         // �̹���
        m_imageRectTransform = m_object.transform.GetChild(3).GetChild(0).GetComponent<RectTransform>();
        m_dataText[(int)TEXTTYPE.TT_FILETYPE] = m_object.transform.GetChild(3).GetChild(3).GetChild(0).GetComponent<TMP_Text>(); // �ؽ�Ʈ ���� ���� ����
        m_dataText[(int)TEXTTYPE.TT_SIZE]     = m_object.transform.GetChild(3).GetChild(3).GetChild(2).GetComponent<TMP_Text>(); // �ؽ�Ʈ ���� ũ��
        m_dataText[(int)TEXTTYPE.TT_DISKSIZE] = m_object.transform.GetChild(3).GetChild(3).GetChild(3).GetComponent<TMP_Text>(); // �ؽ�Ʈ ���� ��ũ �Ҵ�

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

    #region ����
    public void Remove_SelectData() // ������ ���� ������ ����
    {
        WindowManager WM = GameManager.Ins.Window;
        WindowFile file = WM.Get_WindowFile(WM.Picture.Get_Path());
        if (file == null)
            return;

        string directoryPath = Path.GetDirectoryName(file.FilePath);
        // ��ο� ���� ����
        if (directoryPath == WM.BackgroundPath) // ����ȭ���� ��
        {
            // ����ȭ�� ������ ����
            WM.FileIconSlots.Remove_FileIcon(file.FilePath);
        }
        else // ����ȭ���� �ƴ� ��
        {
            // ���� ����� �θ� ���� �ڽ� ����Ʈ���� ����
            WindowFile parentfile = WM.Get_WindowFile(directoryPath);
            parentfile.Remove_ChildFile(file.FileData);
        }

        // ��ųʸ����� �ش� Ű�� ����
        WM.FileData.Remove(file.FilePath);

        // ���������� ��� ���� �� ���߰�
        string trashbinPath = WM.BackgroundPath + "\\" + "������";
        WindowFile newPathFile = WM.Get_WindowFile(WM.Get_FullFilePath(trashbinPath, file.FileData.fileName), file.FileData);
        newPathFile.Set_PrevfilePath(file.FilePath);

        // ������ �ڽ����� ���
        WindowFile trashbinFile = WM.Get_WindowFile(trashbinPath);
        trashbinFile.Add_ChildFile(file.FileData);

        // (����) �����ΰ��⸦ �� �� ������ �ش� �̹����� ����
        // ������ ��Ȱ��ȭ
        Active_Popup(false);
    }
    #endregion

    #region �ڷΰ���/ �����ΰ���
    public void Folder_Back() // �ڷΰ���
    {
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        var fileList = filedatas.OrderBy(f => f.Key).ToList();
        string currentPath = Get_Path();
        string commonFolderPath = GameManager.Ins.Window.Get_ParentPath(currentPath);
        int currentIndex = fileList.FindIndex(f => f.Value.FilePath == currentPath);

        if (currentIndex > 0) // ù ��° ������ �ƴ� ��쿡�� Ž��
        {
            // ���� ���� ���� ���Ϻ��� �������� Ž���Ͽ� ���� ���� ���� �̹��� ������ ã��
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

    public void Folder_Again() // �����ΰ���
    {
        Dictionary<string, WindowFile> filedatas = GameManager.Ins.Window.FileData;
        var fileList = filedatas.OrderBy(f => f.Key).ToList();
        string currentPath = Get_Path();
        string commonFolderPath = GameManager.Ins.Window.Get_ParentPath(currentPath);
        int currentIndex = fileList.FindIndex(f => f.Value.FilePath == currentPath);

        if (currentIndex >= 0 && currentIndex < fileList.Count - 1) // ������ ������ �ƴ� ��쿡�� Ž��
        {
            // ���� ���� ���� ���Ϻ��� ���������� Ž���Ͽ� ���� ���� ���� �̹��� ������ ã��
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

        // �ڷ� ���� ���� ���� Ȯ��
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

        // ������ ���� ���� ���� Ȯ��
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

        // ��ư �̹��� ������Ʈ
        m_pathButtons[(int)BUTTONTYPE.BT_TL].sprite = canGoBack ? m_pathButtonSprites[(int)IMAGETYPE.IT_TON] : m_pathButtonSprites[(int)IMAGETYPE.IT_TOFF];
        m_pathButtons[(int)BUTTONTYPE.BT_TR].sprite = canGoForward ? m_pathButtonSprites[(int)IMAGETYPE.IT_TON] : m_pathButtonSprites[(int)IMAGETYPE.IT_TOFF];
        m_pathButtons[(int)BUTTONTYPE.BT_IL].sprite = canGoBack ? m_pathButtonSprites[(int)IMAGETYPE.IT_ION] : m_pathButtonSprites[(int)IMAGETYPE.IT_IOFF];
        m_pathButtons[(int)BUTTONTYPE.BT_IR].sprite = canGoForward ? m_pathButtonSprites[(int)IMAGETYPE.IT_ION] : m_pathButtonSprites[(int)IMAGETYPE.IT_IOFF];
    }
    #endregion
}
