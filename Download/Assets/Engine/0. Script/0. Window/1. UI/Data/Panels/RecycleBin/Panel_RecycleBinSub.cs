using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_RecycleBinSub : Panel_Popup
{
    private TMP_Text m_frameText;
    private Image    m_iconImage;
    private TMP_Text m_detailText;
    private TMP_Text m_confirmText;
    private TMP_Text m_cancelText;

    private Sprite m_warningSprite;

    public Panel_RecycleBinSub() : base()
    {
        m_fileType = WindowManager.FILETYPE.TYPE_TRASHSUB;

        m_warningSprite = GameManager.Ins.Resource.Load<Sprite>("1. Graphic/2D/0. Window/Window Recycle bin/UI_Window_RecycleBin_WarningIcon");
    }

    protected override void Active_Event(bool active)
    {
        if(active == true)
        {
            switch(m_activeType)
            {
                case (int)Panel_RecycleBin.BUTTONTYPE.BT_DELETE:
                    WindowManager WMD = GameManager.Ins.Window;
                    WindowFile windowFileD = WMD.Recyclebin.SelectFolderBoxs[0].FileData;

                    m_frameText.text = "���� �׸� ����";
                    m_iconImage.sprite = WMD.Get_FileSprite(windowFileD.FileData.fileType);
                    m_detailText.text = $"���� '{windowFileD.FileData.fileName}'��(��) �����Ͻðڽ��ϱ�?\n�� ������ �����뿡�� ������ ������� �Ǹ�, ������ �Ұ����մϴ�.";
                    m_confirmText.text = "����";
                    m_cancelText.text = "���";
                    break;

                case (int)Panel_RecycleBin.BUTTONTYPE.BT_RESTORE:
                    //WindowManager WMR = GameManager.Ins.Window;
                    //WindowFile windowFileR = WMR.Recyclebin.SelectFolderBoxs[0].FileData;

                    //m_frameText.text = "���� �׸� ����";
                    //m_iconImage.sprite = WMR.Get_FileSprite(windowFileR.FileData.fileType);
                    //m_detailText.text = $"���� '{windowFileR.FileData.fileName}'��(��) �����Ͻðڽ��ϱ�?";
                    //m_confirmText.text = "����";
                    //m_cancelText.text = "���";r
                    break;

                case (int)Panel_RecycleBin.BUTTONTYPE.BT_DELETEALL:
                    m_frameText.text = "���� �׸� ����";
                    m_iconImage.sprite = m_warningSprite;
                    m_detailText.text = "������ �׸��� ��� ���� �����Ͻðڽ��ϱ�?";
                    m_confirmText.text = "����";
                    m_cancelText.text = "���";
                    break;

                case (int)Panel_RecycleBin.BUTTONTYPE.BT_RESTOREALL:
                    m_frameText.text = "���� �׸� ����";
                    m_iconImage.sprite = m_warningSprite;
                    m_detailText.text = "������ �׸��� ��� �����Ͻðڽ��ϱ�?";
                    m_confirmText.text = "����";
                    m_cancelText.text = "���";
                    break;
            }
        }
        else
        {
            GameManager.Ins.Window.Recyclebin.Reset_SelectBox();
        }
    }

    public override void Load_Scene()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        m_object = GameManager.Ins.Resource.LoadCreate("5. Prefab/0. Window/UI/Recycle/Panel_RecycleBinSub", canvas.GetChild(3));
        m_select = false;
        m_object.SetActive(m_select);

        // ��ư �̺�Ʈ �߰�
        m_object.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => Active_Popup(false));

        #region �⺻ ����
        m_object.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => Button_Confirm());// Ȯ��
        m_object.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Button_Cancel()); // ���

        m_frameText   = m_object.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>(); // ������ �̸� ����
        m_iconImage   = m_object.transform.GetChild(2).GetComponent<Image>();                // ������ �̹��� ����
        m_detailText  = m_object.transform.GetChild(3).GetComponent<TMP_Text>();             // ���� ����
        m_confirmText = m_object.transform.GetChild(4).GetChild(0).GetComponent<TMP_Text>(); // Ȯ�� �ؽ�Ʈ
        m_cancelText  = m_object.transform.GetChild(5).GetChild(0).GetComponent<TMP_Text>(); // ��� �ؽ�Ʈ
        #endregion
    }

    public void Button_Confirm()
    {
        GameManager.Ins.Window.Recyclebin.Button_Event((Panel_RecycleBin.BUTTONTYPE)m_activeType);
    }

    public void Button_Cancel()
    {
        GameManager.Ins.Window.Recyclebin.Reset_SelectBox();
        Active_Popup(false);
    }
}
