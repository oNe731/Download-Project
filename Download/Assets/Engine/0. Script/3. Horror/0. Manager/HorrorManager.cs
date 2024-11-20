using System.Collections.Generic;
using UnityEngine;

using Horror;

public class HorrorManager : StageManager
{
    public enum LEVEL { LV_1STAGE, LV_2STAGE, LV_END };

    private Dictionary<string, Sprite> m_noteElementIcon = new Dictionary<string, Sprite>();

    private HorrorPlayer m_player;
    private UIPopup m_popupUI = null;
    private UIInstruction m_instructionUI = null;

    public Dictionary<string, Sprite> NoteElementIcon => m_noteElementIcon;
    public HorrorPlayer Player => m_player;
    public UIInstruction InstructionUI => m_instructionUI;

    public HorrorManager() : base()
    {
        m_stageLevel = STAGE.LEVEL_HORROR;
        m_sceneName = "Horror";
    }

    protected override void Load_Resource()
    {
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
        m_noteElementIcon.Add("Icon_Key_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Item/Key/Icon_Key_1"));
        m_noteElementIcon.Add("Icon_Key_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Item/Key/Icon_Key_2"));

        m_noteElementIcon.Add("Icon_A306_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A306/UI_horror_ClueItem_A306File_1"));
        m_noteElementIcon.Add("Icon_A306_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A306/UI_horror_ClueItem_A306File_2"));
        m_noteElementIcon.Add("Icon_A306_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A306/UI_horror_ClueItem_A306File_3"));

        m_noteElementIcon.Add("Icon_A440_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A440/UI_horror_ClueItem_A440_1"));
        m_noteElementIcon.Add("Icon_A440_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A440/UI_horror_ClueItem_A440_2"));
        m_noteElementIcon.Add("Icon_A440_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/A440/UI_horror_ClueItem_A440_3"));

        m_noteElementIcon.Add("Icon_clueNumber_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/ClueNumber/Icon_clueNumber_1"));
        m_noteElementIcon.Add("Icon_clueNumber_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/ClueNumber/Icon_clueNumber_2"));
        m_noteElementIcon.Add("Icon_clueNumber_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/ClueNumber/Icon_clueNumber_3"));

        m_noteElementIcon.Add("Icon_EP14-2_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/EP14-2/UI_horror_ClueItem_EP14-2_1"));
        m_noteElementIcon.Add("Icon_EP14-2_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/EP14-2/UI_horror_ClueItem_EP14-2_2"));
        m_noteElementIcon.Add("Icon_EP14-2_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/EP14-2/UI_horror_ClueItem_EP14-2_3"));

        m_noteElementIcon.Add("Icon_S1_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S1/UI_horror_ClueItem_S1_1"));
        m_noteElementIcon.Add("Icon_S1_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S1/UI_horror_ClueItem_S1_2"));
        m_noteElementIcon.Add("Icon_S1_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S1/UI_horror_ClueItem_S1_3"));

        m_noteElementIcon.Add("Icon_S2_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S2/UI_horror_ClueItem_S2_1"));
        m_noteElementIcon.Add("Icon_S2_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S2/UI_horror_ClueItem_S2_2"));
        m_noteElementIcon.Add("Icon_S2_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S2/UI_horror_ClueItem_S2_3"));

        m_noteElementIcon.Add("Icon_S3_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S3/UI_horror_ClueItem_S3_1"));
        m_noteElementIcon.Add("Icon_S3_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S3/UI_horror_ClueItem_S3_2"));
        m_noteElementIcon.Add("Icon_S3_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S3/UI_horror_ClueItem_S3_3"));

        m_noteElementIcon.Add("Icon_S4_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S4/UI_horror_ClueItem_S4_1"));
        m_noteElementIcon.Add("Icon_S4_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S4/UI_horror_ClueItem_S4_2"));
        m_noteElementIcon.Add("Icon_S4_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S4/UI_horror_ClueItem_S4_3"));

        m_noteElementIcon.Add("Icon_S5_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S5/UI_horror_ClueItem_S5_1"));
        m_noteElementIcon.Add("Icon_S5_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S5/UI_horror_ClueItem_S5_2"));
        m_noteElementIcon.Add("Icon_S5_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S5/UI_horror_ClueItem_S5_3"));

        m_noteElementIcon.Add("Icon_S6_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S6/UI_horror_ClueItem_S6_1"));
        m_noteElementIcon.Add("Icon_S6_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S6/UI_horror_ClueItem_S6_2"));
        m_noteElementIcon.Add("Icon_S6_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S6/UI_horror_ClueItem_S6_3"));

        m_noteElementIcon.Add("Icon_S7_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S7/UI_horror_ClueItem_S7_1"));
        m_noteElementIcon.Add("Icon_S7_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S7/UI_horror_ClueItem_S7_2"));
        m_noteElementIcon.Add("Icon_S7_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S7/UI_horror_ClueItem_S7_3"));

        m_noteElementIcon.Add("Icon_S8_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S8/UI_horror_ClueItem_S8_1"));
        m_noteElementIcon.Add("Icon_S8_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S8/UI_horror_ClueItem_S8_2"));
        m_noteElementIcon.Add("Icon_S8_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S8/UI_horror_ClueItem_S8_3"));

        m_noteElementIcon.Add("Icon_S9_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S9/UI_horror_ClueItem_S9_1"));
        m_noteElementIcon.Add("Icon_S9_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S9/UI_horror_ClueItem_S9_2"));
        m_noteElementIcon.Add("Icon_S9_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S9/UI_horror_ClueItem_S9_3"));

        m_noteElementIcon.Add("Icon_S10_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S10/UI_horror_ClueItem_S10_1"));
        m_noteElementIcon.Add("Icon_S10_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S10/UI_horror_ClueItem_S10_2"));
        m_noteElementIcon.Add("Icon_S10_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S10/UI_horror_ClueItem_S10_3"));

        m_noteElementIcon.Add("Icon_S11_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S11/UI_horror_ClueItem_S11_1"));
        m_noteElementIcon.Add("Icon_S11_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S11/UI_horror_ClueItem_S11_2"));
        m_noteElementIcon.Add("Icon_S11_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S11/UI_horror_ClueItem_S11_3"));

        m_noteElementIcon.Add("Icon_S12_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S12/UI_horror_ClueItem_S12_1"));
        m_noteElementIcon.Add("Icon_S12_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S12/UI_horror_ClueItem_S12_2"));
        m_noteElementIcon.Add("Icon_S12_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S12/UI_horror_ClueItem_S12_3"));

        m_noteElementIcon.Add("Icon_S13_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S13/UI_horror_ClueItem_S13_1"));
        m_noteElementIcon.Add("Icon_S13_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S13/UI_horror_ClueItem_S13_2"));
        m_noteElementIcon.Add("Icon_S13_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S13/UI_horror_ClueItem_S13_3"));

        m_noteElementIcon.Add("Icon_S14_1", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S14/UI_horror_ClueItem_S14_1"));
        m_noteElementIcon.Add("Icon_S14_2", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S14/UI_horror_ClueItem_S14_2"));
        m_noteElementIcon.Add("Icon_S14_3", GameManager.Ins.Resource.Load<Sprite>(basicPath + "Clue/S14/UI_horror_ClueItem_S14_3"));
    }

    public override void Enter_Stage()
    {
        base.Enter_Stage();
    }

    protected override void Load_Scene()
    {
        // 변수 할당
        GameObject playergameObject = GameObject.FindWithTag("Player");
        if (playergameObject != null)
            m_player = playergameObject.GetComponent<HorrorPlayer>();

        // 기본 값 초기화
        GameObject popup = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Popup", GameObject.Find("Canvas").transform.Find("Panel_Middle"));
        if (popup == null)
            return;
        m_popupUI = popup.GetComponent<UIPopup>();
        m_popupUI.gameObject.SetActive(false);

        GameObject instruction = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_Instruction", GameObject.Find("Canvas").transform.Find("Panel_Middle"));
        if (instruction == null)
            return;
        m_instructionUI = instruction.GetComponent<UIInstruction>();
        m_instructionUI.gameObject.SetActive(false);

        // 레벨 초기화
        m_levelController = new LevelController();
        List<Level> levels = new List<Level>
        {
            new Horror_1stage(),
            new Horror_2stage(),
        };
        for (int i = 0; i < levels.Count; ++i)
            levels[i].Initialize_Level(m_levelController);
        m_levelController.Initialize_Level(levels, (int)LEVEL.LV_1STAGE);

        // 게임 시작
        Cursor.lockState = CursorLockMode.None;
        GameManager.Ins.UI.EventUpdate = true;
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
    }

    public override void Update_Stage()
    {
        if (m_levelController == null)
            return;

        m_levelController.Update_Level();

//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.F2))
//            m_levelController.Change_Level((int)LEVEL.LV_2STAGE);
//#endif
    }

    public override void LateUpdate_Stage()
    {
        if (m_levelController == null)
            return;

        m_levelController.LateUpdate_Level();
    }

    public override void Exit_Stage()
    {
        base.Exit_Stage();
    }

    public override void Set_Pause(bool pause, bool Setcursur)
    {
        base.Set_Pause(pause, Setcursur);
        if (Player != null)
            Player.Stop_Player(pause);
    }


    public void Start_Game()
    {
        GameManager.Ins.UI.Start_FadeIn(1f, Color.black);
        GameManager.Ins.IsGame = true;

        // 카메라 설정
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return;
        GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_FOLLOW);
        CameraFollow camera = (CameraFollow)GameManager.Ins.Camera.Get_CurCamera();
        camera.Set_FollowInfo(player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).transform, player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).transform, false, false, new Vector3(0.0f, 1.3f, 0.0f), 200.0f, 30.0f, new Vector2(-45f, 45f), true, true);
    }

    public GameObject Create_WorldHintUI(UIWorldHint.HINTTYPE hinttype, Transform target, Vector3 m_uiOffset)
    {
        GameObject gameObject = GameManager.Ins.Resource.LoadCreate("5. Prefab/3. Horror/UI/UI_WorldHint");//, LevelController.Get_CurrentLevel<Horror_Base>().Stage.transform);
        if (gameObject == null)
            return null;

        gameObject.GetComponent<UIWorldHint>().Initialize_UI(hinttype, target, m_uiOffset);
        return gameObject;
    }

    public void Active_Popup(UIPopup.TYPE type, NoteItem itemType, bool closeText = false, float[] activeTimes = null, string[] texts = null, bool closeEvent = false, UIPopup.EVENT eventType = UIPopup.EVENT.E_END)
    {
        if (m_popupUI == null)
            return;

        m_popupUI.Initialize_UI(type, itemType, closeText, activeTimes, texts, closeEvent, eventType);
    }

    public void Active_InstructionUI(UIInstruction.ACTIVETYPE openType, UIInstruction.ACTIVETYPE closeType, float[] activeTimes, string[] texts) // 안내 문구 출력
    {
        if (m_instructionUI == null || m_instructionUI.Check_Text(texts) == false)
            return;

        m_instructionUI.Initialize_UI(openType, closeType, activeTimes, texts);
    }

    public void Over_Game()
    {
        if (GameManager.Ins.IsGame == false) return;

        GameManager.Ins.Set_Pause(true, false);
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
        GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_HORROR);
    }
}
