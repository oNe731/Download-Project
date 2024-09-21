using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Horror;

public class HorrorManager : MonoBehaviour
{
    public enum LEVEL { LV_1STAGE, LV_2STAGE, LV_END };

    private static HorrorManager m_instance = null;
    private LevelController m_levelController = null;

    private bool m_isGame = false;
    private HorrorPlayer m_player;
    private UIPopup m_popupUI = null;
    private UIInstruction m_instructionUI = null;
    private Dictionary<string, Sprite> m_noteElementIcon = new Dictionary<string, Sprite>();

    public static HorrorManager Instance => m_instance;
    public LevelController LevelController => m_levelController;
    public bool IsGame => m_isGame;
    public HorrorPlayer Player => m_player;
    public Dictionary<string, Sprite> NoteElementIcon => m_noteElementIcon;
    public UIInstruction InstructionUI => m_instructionUI;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;

        GameObject playergameObject = GameObject.FindWithTag("Player");
        if (playergameObject != null)
            m_player = playergameObject.GetComponent<HorrorPlayer>();

        m_levelController = gameObject.AddComponent<LevelController>();

        List<Level> levels = new List<Level>
        {
            gameObject.AddComponent<Horror_1stage>(),
            gameObject.AddComponent<Horror_2stage>(),
        };

        gameObject.GetComponent<Horror_1stage>().Initialize_Level(m_levelController);
        gameObject.GetComponent<Horror_2stage>().Initialize_Level(m_levelController);

        m_levelController.Initialize_Level(levels, (int)LEVEL.LV_1STAGE);

        string basicPath = "1. Graphic/2D/3. Horror/UI/Play/Icon/";
        m_noteElementIcon.Add("Icon_None_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Etc/None/Icon_None_1"));
        m_noteElementIcon.Add("Icon_None_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Etc/None/Icon_None_2"));
        m_noteElementIcon.Add("Icon_None_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Etc/None/Icon_None_3"));

        m_noteElementIcon.Add("Icon_Note_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Basic/Note/Icon_Note_1"));

        m_noteElementIcon.Add("Icon_Stick_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Weapon/Stick/Icon_Stick_1"));
        m_noteElementIcon.Add("Icon_Stick_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Weapon/Stick/Icon_Stick_2"));
        m_noteElementIcon.Add("Icon_Gun_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Weapon/Gun/Icon_Gun_1"));
        m_noteElementIcon.Add("Icon_Gun_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Weapon/Gun/Icon_Gun_2"));
        m_noteElementIcon.Add("Icon_Flashlight_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Weapon/Flashlight/Icon_Flashlight_1"));
        m_noteElementIcon.Add("Icon_Flashlight_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Weapon/Flashlight/Icon_Flashlight_2"));

        m_noteElementIcon.Add("Icon_Bullet_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Item/Bullet/Icon_Bullet_1"));
        m_noteElementIcon.Add("Icon_Bullet_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Item/Bullet/Icon_Bullet_2"));
        m_noteElementIcon.Add("Icon_Medicine_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Item/Medicine/Icon_Medicine_1"));
        m_noteElementIcon.Add("Icon_Medicine_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Item/Medicine/Icon_Medicine_2"));

        m_noteElementIcon.Add("Icon_clue_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/Clue/Icon_clue_1"));
        m_noteElementIcon.Add("Icon_clue_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/Clue/Icon_clue_2"));
        m_noteElementIcon.Add("Icon_clue_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/Clue/Icon_clue_3"));
        m_noteElementIcon.Add("Icon_A306File_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A306File/UI_horror_ClueItem_A306File_1"));
        m_noteElementIcon.Add("Icon_A306File_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A306File/UI_horror_ClueItem_A306File_2"));
        m_noteElementIcon.Add("Icon_A306File_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A306File/UI_horror_ClueItem_A306File_3"));
        m_noteElementIcon.Add("Icon_clueNumber_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/ClueNumber/Icon_clueNumber_1"));
        m_noteElementIcon.Add("Icon_clueNumber_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/ClueNumber/Icon_clueNumber_2"));
        m_noteElementIcon.Add("Icon_clueNumber_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/ClueNumber/Icon_clueNumber_3"));

        // 사용할 UI 생성
        if (m_popupUI == null)
        {
            GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.Find("Panel_Middle"));
            if (gameObject == null)
                return;
            m_popupUI = gameObject.GetComponent<UIPopup>();
            m_popupUI.gameObject.SetActive(false);
        }
        if (m_instructionUI == null)
        {
            GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Instruction", GameObject.Find("Canvas").transform.Find("Panel_Middle"));
            if (gameObject == null)
                return;
            m_instructionUI = gameObject.GetComponent<UIInstruction>();
            m_instructionUI.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
    }

    private void Update()
    {
        m_levelController.Update_Level();
    }

    public void Start_Game()
    {
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
        m_isGame = true;

        // 카메라 설정
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return;
        GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
        CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
        camera.Set_FollowInfo(player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).transform, player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).transform, false, false, new Vector3(0.0f, 1.3f, 0.0f), 200.0f, 30.0f, new Vector2(-45f, 45f), true, true);
    }

    public void Set_Pause(bool pause, bool Setcursur = true)
    {
        m_isGame = !pause;
        Player.Stop_Player(pause);

        if (Setcursur == true)
            GameManager.Ins.Camera.Set_CursorLock(!pause);
    }

    public GameObject Create_WorldHintUI(UIWorldHint.HINTTYPE hinttype, Transform target, Vector3 m_uiOffset)
    {
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_WorldHint", LevelController.Get_CurrentLevel<Horror_Base>().Stage.transform);
        if (gameObject == null)
            return null;

        gameObject.GetComponent<UIWorldHint>().Initialize_UI(hinttype, target, m_uiOffset);
        return gameObject;
    }

    public void Active_Popup(UIPopup.TYPE type, NoteItem itemType)
    {
        if (m_popupUI == null)
            return;

        m_popupUI.Initialize_UI(type, itemType);
    }

    public void Active_InstructionUI(UIInstruction.ACTIVETYPE openType, UIInstruction.ACTIVETYPE closeType, float[] activeTimes, string[] texts) // 안내 문구 출력
    {
        if (m_instructionUI == null)
            return;

        m_instructionUI.Initialize_UI(openType, closeType, activeTimes, texts);
    }

    public void Over_Game()
    {
        if (IsGame == false) return;

        Set_Pause(true, false);
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/Canvas_GameOver");
        if (gameObject == null)
            return;
        gameObject.GetComponent<UIGameOver>().Start_GameOver();
    }

    public void Restart_Game()
    {
        // 체크포인트에서 게임 재시작
        //GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
        //HorrorManager.Instance.Set_Pause(false, false);
        GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_END);
        GameManager.Ins.Change_Scene("Horror");
    }
}
