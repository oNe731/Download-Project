using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace VisualNovel
{
    public class Dialog_VN : Dialog<DialogData_VN>
    {
        [Header("GameObject")]
        [SerializeField] private GameObject m_darkPanelObj;
        [SerializeField] private GameObject m_backgroundObj;
        [SerializeField] private GameObject[] m_standingObj;
        [SerializeField] private GameObject m_dialogBoxObj;
        [SerializeField] private TMP_Text m_nameTxt;
        [SerializeField] private TMP_Text m_dialogTxt;
        [SerializeField] private NpcLike m_heartScr;

        private Image   m_backgroundImg;
        private Image[] m_standingImg;

        private int              m_choiceIndex = 0;
        private List<GameObject> m_choice_Button = new List<GameObject>();

        private bool m_cutScene = false;
        public bool CutScene { set => m_cutScene = value; }

        private void Awake()
        {
            m_typeSpeed = 0.045f;

            m_backgroundImg = m_backgroundObj.GetComponent<Image>();

            m_standingImg = new Image[m_standingObj.Length];
            for (int i = 0; i < m_standingObj.Length; i++)
                m_standingImg[i] = m_standingObj[i].GetComponent<Image>();
        }

        private void Update()
        {
            /*if(Input.GetKeyDown(KeyCode.F1))
            {
                m_dialogIndex = m_dialogs.Count - 1;
                m_isTyping    = false;
                Update_Dialog();
            }*/

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
                Update_Dialogs();

            // 버튼 업데이트
            if (0 < m_choice_Button.Count)
                Update_Button();
        }

        private void Update_Dialogs(bool IsPonter = true)
        {
            // 커서가 UI에 존재할 시 업데이트 방지
            if (IsPonter && EventSystem.current.IsPointerOverGameObject())
                return;

            if (m_isTyping)
            {
                m_cancelTyping = true;
            }
            else // 다이얼로그 진행
            {
                if (m_dialogIndex < m_dialogs.Count)
                {
                    switch (m_dialogs[m_dialogIndex].dialogType)
                    {
                        case DialogData_VN.DIALOG_TYPE.DT_FADE:
                            Update_Fade();
                            break;

                        case DialogData_VN.DIALOG_TYPE.DT_DIALOG:
                            Update_Dialog(m_dialogIndex);
                            break;

                        case DialogData_VN.DIALOG_TYPE.DT_GAMESTATE:
                            Update_GameState();
                            break;

                        case DialogData_VN.DIALOG_TYPE.DT_CUTSCENE:
                            Update_CutScene();
                            break;
                    }
                }
                else // 다이얼로그 종료
                {
                    if (0 < m_choice_Button.Count)
                        return;

                    Close_Dialog();
                }
            }
        }

        #region Fade
        private void Update_Fade()
        {
            FadeData fadeData = (FadeData)m_dialogs[m_dialogIndex].dialogSubData;
            switch(fadeData.fadeType)
            {
                case FadeData.FADETYPE.FT_IN:
                    Update_FadeIn();
                    break;

                case FadeData.FADETYPE.FT_OUT:
                    Update_FadeOut(fadeData);
                    break;

                case FadeData.FADETYPE.FT_INOUT:
                    Update_FadeInOut(fadeData);
                    break;

                case FadeData.FADETYPE.FT_OUTIN:
                    Update_FadeOutIn();
                    break;
            }
        }

        private void Update_FadeIn() // 다이얼로그, 추격
        {
            // 다음 다이얼로그 타입에 따른 처리
            if (m_dialogs[m_dialogIndex + 1].dialogType == DialogData_VN.DIALOG_TYPE.DT_CUTSCENE)
            {
                m_dialogIndex++;
            }
            else // 다음 데이터로 미리 업데이트
            {
                Update_Dialog(m_dialogIndex + 1);
            }

            // 텍스트 초기화
            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_isTyping = false;
            m_dialogTxt.text = "";

            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Update_Dialogs());
        }

        private void Update_FadeOut(FadeData fadeData)
        {
            if (string.IsNullOrEmpty(fadeData.path))
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black);
            else
                GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>(fadeData.path)), 0f, false);
        }

        private void Update_FadeInOut(FadeData fadeData)
        {
            GameManager.Ins.UI.Start_FadeIn(1f, Color.black, () => Update_FadeOut(fadeData), 0.5f, false);
        }

        private void Update_FadeOutIn()
        {
            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, () => Update_FadeIn(), 0.5f, false);
        }
        #endregion

        #region Dialog
        private void Update_DialogBasic(VisualNovelManager.OWNERTYPE ownerType)
        {
            // 다이얼로그 박스 활성화
            m_dialogBoxObj.SetActive(true);

            // 호감도 업데이트
            m_heartScr.Set_Owner(ownerType);

            // 다이얼로그 오너 이름 업데이트
            switch (ownerType)
            {
                case VisualNovelManager.OWNERTYPE.OT_WHITE:
                    m_nameTxt.text = GameManager.Ins.PlayerName;
                    break;

                case VisualNovelManager.OWNERTYPE.OT_BLUE:
                    m_nameTxt.text = "미나츠";
                    break;

                case VisualNovelManager.OWNERTYPE.OT_YELLOW:
                    m_nameTxt.text = "히나";
                    break;

                case VisualNovelManager.OWNERTYPE.OT_PINK:
                    m_nameTxt.text = "아야카";
                    break;
            }
        }

        private void Update_Dialog(int dialogIndex, bool nextUpdate = false)
        {
            DialogData dialogData = (DialogData)m_dialogs[dialogIndex].dialogSubData;

            // 기본 정보 업데이트
            Update_DialogBasic(dialogData.owner);

            // 리소스 업데이트 : 배경, 스탠딩
            if (!string.IsNullOrEmpty(dialogData.backgroundSpr))
                m_backgroundImg.sprite = GameManager.Ins.Novel.BackgroundSpr[dialogData.backgroundSpr];
            Update_Standing(dialogData.standingSpr);

            // 타이핑 업데이트
            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_Text(dialogData, nextUpdate));

            m_dialogIndex++;
        }

        private void Update_Standing(List<string> standingSpr)
        {
            switch (standingSpr.Count)
            {
                case 0:
                    m_standingObj[0].SetActive(false);
                    m_standingObj[1].SetActive(false);
                    m_standingObj[2].SetActive(false);
                    break;

                case 1:
                    if (!string.IsNullOrEmpty(standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[0]];
                    }

                    m_standingObj[1].SetActive(false);
                    m_standingObj[2].SetActive(false);
                    break;

                case 2:
                    if (!string.IsNullOrEmpty(standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[0]];
                    }

                    if (!string.IsNullOrEmpty(standingSpr[1]))
                    {
                        m_standingObj[1].SetActive(true);
                        m_standingObj[1].transform.localPosition = new Vector3(300.0f, -460.0f, 0.0f);
                        m_standingImg[1].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[1]];
                    }
                    m_standingObj[2].SetActive(false);
                    break;

                case 3:
                    if (!string.IsNullOrEmpty(standingSpr[0]))
                    {
                        m_standingObj[0].SetActive(true);
                        m_standingObj[0].transform.localPosition = new Vector3(0.0f, -460.0f, 0.0f);
                        m_standingImg[0].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[0]];
                    }

                    if (!string.IsNullOrEmpty(standingSpr[1]))
                    {
                        m_standingObj[1].SetActive(true);
                        m_standingObj[1].transform.localPosition = new Vector3(300.0f, -460.0f, 0.0f);
                        m_standingImg[1].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[1]];
                    }

                    if (!string.IsNullOrEmpty(standingSpr[2]))
                    {
                        m_standingObj[2].SetActive(true);
                        m_standingObj[2].transform.localPosition = new Vector3(500.0f, -460.0f, 0.0f);
                        m_standingImg[2].sprite = GameManager.Ins.Novel.StandingSpr[standingSpr[2]];
                    }
                    break;
            }
        }

        #region Button
        private void Create_ChoiceButton(ChoiceData choiceData)
        {
            // 검은 배경 활성화
            m_darkPanelObj.SetActive(true);

            // 선택지 버튼 생성
            for (int i = 0; i < choiceData.choiceText.Count; ++i)
            {
                int ButtonIndex = i + 1; // 버튼 고유 인덱스

                GameObject Clone = GameManager.Ins.Resource.LoadCreate("5. Prefab/1. VisualNovel/UI/Button_Choice_VN");
                if (Clone != null)
                {
                    Clone.transform.SetParent(gameObject.transform);
                    Clone.transform.localPosition = new Vector3(0f, (130 + (i * -130)), 0f); // 130 / 0 / -130
                    Clone.transform.localScale    = new Vector3(1f, 1f, 1f);

                    ButtonChoice_VN ButtonChoice = Clone.GetComponent<ButtonChoice_VN>();
                    ButtonChoice.ButtonIndex = i;
                    ButtonChoice.Ownerdialog = this;

                    TMP_Text TextCom = Clone.GetComponentInChildren<TMP_Text>();
                    if (TextCom)
                    {
                        TextCom.text = choiceData.choiceText[i];

                        Button button = Clone.GetComponent<Button>();
                        if (button != null) // 이벤트 핸들러 추가
                            button.onClick.AddListener(() => Click_Button(ButtonIndex));

                        m_choice_Button.Add(Clone);
                    }
                }
            }

            // 기본 선택 상태 설정
            m_choiceIndex = 0;
            m_choice_Button[m_choiceIndex].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"];
        }

        private void Update_Button()
        {
            if (m_isTyping == true)
                return;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Click_Button(m_choiceIndex);
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_choiceIndex--;
                if (m_choiceIndex < 0)
                    m_choiceIndex = m_choice_Button.Count - 1;

                Update_ButtonImage();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_choiceIndex++;
                if (m_choiceIndex > m_choice_Button.Count - 1)
                    m_choiceIndex = 0;

                Update_ButtonImage();
            }
        }

        private void Click_Button(int index)
        {
            DialogData dialogData = (DialogData)m_dialogs[m_dialogIndex - 1].dialogSubData;
            ChoiceData choiceData = dialogData.choiceData;
            switch (choiceData.choiceEventType[index - 1])
            {
                case ChoiceData.CHOICETYPE.CT_CLOSE: // 다이얼로그 종료
                    Close_Dialog();
                    break;

                case ChoiceData.CHOICETYPE.CT_DIALOG: // 다이얼로그 재시작
                    Start_Dialog(GameManager.Ins.Load_JsonData<DialogData_VN>(choiceData.choiceDialog[index - 1]));
                    break;
            }
        }

        public void Update_ButtonImage()
        {
            // 현재 인덱스 버튼을 제외한 모든 버튼 Off 이미지로 초기화
            for (int i = 0; i < m_choice_Button.Count; ++i)
            {
                if (i == m_choiceIndex)
                    m_choice_Button[i].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonON"]; // On
                else
                    m_choice_Button[i].GetComponent<Image>().sprite = GameManager.Ins.Novel.ChoiceButtonSpr["UI_VisualNovel_White_ButtonOFF"]; // Off
            }
        }

        public void Enter_Button(int index)
        {
            m_choiceIndex = index;
            Update_ButtonImage();
        }
        #endregion
        #endregion

        #region GameState
        private void Update_GameState()
        {
            GameState gameState = (GameState)m_dialogs[m_dialogIndex].dialogSubData;

            Action action = null;
            switch (gameState.gameType)
            {
                case GameState.GAMETYPE.GT_STARTSHOOT:
                    action = () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_SHOOTGAME);
                    break;

                case GameState.GAMETYPE.GT_STARTCHASE:
                    action = () => GameManager.Ins.Novel.LevelController.Change_Level((int)VisualNovelManager.LEVELSTATE.LS_CHASEGAME);
                    break;

                case GameState.GAMETYPE.GT_PLAYCHASE:
                    action = () => GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Chase>().Play_Level();
                    break;
            }

            GameManager.Ins.UI.Start_FadeOut(1f, Color.black, action, 0.5f, false);
        }
        #endregion

        #region CutScene
        private void Update_CutScene()
        {
            if (m_cutScene == true)
                return;

            CutScene dialogData = (CutScene)m_dialogs[m_dialogIndex].dialogSubData;

            int eventCount = dialogData.cutSceneEvents.Count;
            if (eventCount <= 0)
                return;

            m_cutScene = true;
            for (int i = 0; i < eventCount; ++i)
            {
                CutScene.CUTSCENETYPE cutSceneEvent = dialogData.cutSceneEvents[i];
                switch (cutSceneEvent)
                {
                    case VisualNovel.CutScene.CUTSCENETYPE.CT_BLINK:
                        Update_Blink((BasicValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_CAMERA:
                        Update_Camera((CameraValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_ANIMATION:
                        Update_Animation((AnimationValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_LIKEPANEL:
                        StartCoroutine(Update_LikePanel());
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_ACTIVE:
                        Update_Active((ActiveValue)dialogData.eventValues[i]);
                        break;

                    case VisualNovel.CutScene.CUTSCENETYPE.CT_IMAGE:
                        Update_Image((ImageValue)dialogData.eventValues[i]);
                        break;
                }
            }
        }

        private void Update_Blink(BasicValue basicValue) // 인 아웃 // 인 아웃 // 인
        {
            m_dialogBoxObj.SetActive(false);

            GameManager.Ins.UI.Start_FadeIn(3.5f, Color.black,
                () => GameManager.Ins.UI.Start_FadeOut(2f, Color.black,

                () => GameManager.Ins.UI.Start_FadeIn(1f, Color.black,
                () => GameManager.Ins.UI.Start_FadeOut(1f, Color.black,

                () => GameManager.Ins.UI.Start_FadeIn(1f, Color.black,
                () => Finish_CutScene(basicValue.nextIndex)), 0f, false)), 0f, false));
        }

        private void Update_Camera(CameraValue cameraValue)
        {
            GameManager.Ins.Camera.Change_Camera(CAMERATYPE.CT_CUTSCENE);

            CameraCutscene camera = (CameraCutscene)GameManager.Ins.Camera.Get_CurCamera();
            if (cameraValue.usePosition == true)
                camera.Start_Position(cameraValue.targetPosition, cameraValue.positionSpeed);
            if (cameraValue.useRotation == true)
                camera.Start_Rotation(cameraValue.targetRotation, cameraValue.rotationSpeed);

            StartCoroutine(Finish_Camera(camera, cameraValue.usePosition, cameraValue.useRotation, cameraValue.nextIndex));
        }

        private void Update_Animation(AnimationValue animationValue)
        {
            switch (animationValue.owner)
            {
                case VisualNovelManager.OWNERTYPE.OT_PINK:
                    // 업데이트 여부 확인
                    if (!string.IsNullOrEmpty(animationValue.animatroTriger))
                    {
                        Novel_Chase novel_Chase = GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Chase>();
                        novel_Chase.YandereAnimator.SetTrigger(animationValue.animatroTriger);
                    }
                    break;
            }

            Update_DialogBasic(animationValue.owner);
            if (m_dialogTextCoroutine != null)
                StopCoroutine(m_dialogTextCoroutine);
            m_dialogTextCoroutine = StartCoroutine(Type_CutText(animationValue));
            m_dialogIndex++;

            m_cutScene = false;
        }

        private IEnumerator Update_LikePanel()
        {
            float time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (time >= 0.5f)
                {
                    GameManager.Ins.Novel.LikeabilityPanel.SetActive(true);
                    break;
                }

                yield return null;
            }

            time = 0f;
            while (true)
            {
                time += Time.deltaTime;
                if (time >= 1.0f)
                {
                    GameManager.Ins.Novel.LikeabilityPanel.GetComponent<Likeability>().Shake_Heart();
                    break;
                }

                yield return null;
            }

            m_dialogIndex++;
            yield break;
        }

        private void Update_Active(ActiveValue activeValue)
        {
            switch (activeValue.objectType)
            {
                case ActiveValue.OBJECT_TYPE.OJ_SAW:
                    GameManager.Ins.Novel.LevelController.Get_CurrentLevel<Novel_Chase>().Yandere.gameObject.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(activeValue.active);
                    break;
            }

            Finish_CutScene(activeValue.nextIndex);
        }

        private void Update_Image(ImageValue iamgeValue)
        {
            // 스탠딩 비활성화
            m_standingObj[0].SetActive(false);
            m_standingObj[1].SetActive(false);
            m_standingObj[2].SetActive(false);

            // 이미지 박스
            m_dialogBoxObj.SetActive(false);

            // 컷씬 이미지 변경
            if (!string.IsNullOrEmpty(iamgeValue.imageName))
                m_backgroundImg.sprite = GameManager.Ins.Novel.CutScene[iamgeValue.imageName];

            m_cutScene = false;
            m_dialogIndex++;
        }

        private void Finish_CutScene(bool nextIndex)
        {
            m_cutScene = false;
            m_dialogIndex++;

            if (nextIndex == false)
                return;
            Update_Dialogs();
        }

        private IEnumerator Finish_Camera(CameraCutscene camera, bool position, bool rotation, bool nextIndex)
        {
            while (true)
            {
                if (position == true && rotation == true)
                {
                    if (camera.IsPosition == false && camera.IsRotation == false)
                        break;
                }
                else if (position == true)
                {
                    if (camera.IsPosition == false)
                        break;
                }
                else if (rotation == true)
                {
                    if (camera.IsRotation == false)
                        break;
                }

                yield return null;
            }

            Finish_CutScene(nextIndex);
            yield break;
        }
        #endregion


        IEnumerator Type_CutText(AnimationValue animationValue)
        {
            // 플레이어 이름이 사용될 시 입력 받은 이름으로 변경
            string dialogText = animationValue.dialogText.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

            m_isTyping = true;
            m_cancelTyping = false;

            m_dialogTxt.text = "";
            foreach (char letter in dialogText.ToCharArray())
            {
                if (m_cancelTyping)
                {
                    m_dialogTxt.text = dialogText;
                    break;
                }

                m_dialogTxt.text += letter;
                yield return new WaitForSeconds(m_typeSpeed);
            }

            m_isTyping = false;

            // 자동 업데이트
            if (animationValue.nextIndex == true)
                Update_Dialogs();

            yield break;
        }

        public void Close_Background()
        {
            m_backgroundObj.SetActive(false);
        }


        #region Common
        public void Start_Dialog(List<DialogData_VN> dialogs = null)
        {
            // 다이얼로그 정보 초기화
            m_dialogs = dialogs;

            // 변수 초기화
            m_isTyping = false;
            m_cancelTyping = false;
            m_dialogIndex = 0;
            m_choiceIndex = 0;

            m_darkPanelObj.SetActive(false);
            m_standingObj[0].SetActive(false);
            m_standingObj[1].SetActive(false);
            m_standingObj[2].SetActive(false);

            for (int i = 0; i < m_choice_Button.Count; ++i)
                Destroy(m_choice_Button[i]);
            m_choice_Button.Clear();

            m_backgroundObj.SetActive(true);
            gameObject.SetActive(true);
            Update_Dialogs(false);
        }

        private void Close_Dialog()
        {
            gameObject.SetActive(false);
        }

        IEnumerator Type_Text(DialogData dialogData, bool nextUpdate)
        {
            // 플레이어 이름이 사용될 시 입력 받은 이름으로 변경
            string dialogText = dialogData.dialogText.Replace("{{PLAYER_NAME}}", GameManager.Ins.PlayerName);

            m_isTyping = true;
            m_cancelTyping = false;

            m_dialogTxt.text = "";
            foreach (char letter in dialogText.ToCharArray())
            {
                if (m_cancelTyping)
                {
                    m_dialogTxt.text = dialogText;
                    break;
                }

                m_dialogTxt.text += letter;
                yield return new WaitForSeconds(m_typeSpeed);
            }

            m_isTyping = false;

            // 선택지 생성
            if (dialogData.choiceData.choiceText != null)
            {
                if (0 < dialogData.choiceData.choiceText.Count)
                    Create_ChoiceButton(dialogData.choiceData);
            }

            // 호감도 증가
            if (dialogData.addLike == true)
            {
                GameManager.Ins.Novel.NpcHeart[(int)dialogData.owner]++;
                m_heartScr.Set_Owner(dialogData.owner);
            }

            // 자동 업데이트
            if (nextUpdate == true)
                Update_Dialogs();

            yield break;
        }
        #endregion
    }
}

