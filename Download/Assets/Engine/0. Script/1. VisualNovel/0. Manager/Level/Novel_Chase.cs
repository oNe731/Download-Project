using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualNovel
{
    public class Novel_Chase : Level
    {
        private GameObject m_yandereObj;
        private List<HallwayLight> m_Light = new List<HallwayLight>(); // 464
        private List<GameObject> m_Levers = new List<GameObject>();

        private HallwayYandere m_yandere;
        private HallwayPlayer m_player;
        private Transform m_yandereTr;
        private Transform m_playerTr;

        private int m_CdMaxCount = 5;
        private int m_CdCurrentCount = 0;
        private int m_LeverMaxCount = 2;
        private float m_CdMinDistance = 20.0f;
        private float m_CdMaxDistance = 200.0f;

        public HallwayPlayer Player { get => m_player; }
        public Transform PlayerTr { get => m_playerTr; }
        public List<HallwayLight> Light
        {
            get => m_Light;
            set => m_Light = value;
        }

        public Novel_Chase(LevelController levelController) : base(levelController)
        {
        }

        public override void Enter_Level()
        {
            m_player = VisualNovelManager.Instance.PlayerObj.GetComponent<HallwayPlayer>();
            m_playerTr = VisualNovelManager.Instance.PlayerObj.GetComponent<Transform>();

            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_BASIC_3D);
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 1.4f, -2.8f));
            camera.Change_Rotation(new Vector3(8.5f, 0f, 0f));

            // 지하실 다이얼로그 시작 (페이드 인)
            Dialog_VN dialog = VisualNovelManager.Instance.Dialog.GetComponent<Dialog_VN>();
            dialog.Start_Dialog(GameManager.Instance.Load_JsonData<DialogData_VN>("Assets/Resources/4. Data/1. VisualNovel/Dialog/Dialog5_Cellar.json"));
            dialog.Close_Background();

            VisualNovelManager.Instance.ChaseGame.SetActive(true);
        }

        public override void Play_Level()
        {
            VisualNovelManager.Instance.Dialog.SetActive(false);

            Create_CD();
            Create_Lever(m_LeverMaxCount);
            m_player.Set_Lock(false);

            UIManager.Instance.Start_FadeIn(1f, Color.black);
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_FOLLOW);
        }

        public override void Update_Level()
        {
        }

        public override void Exit_Level()
        {
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_END);
        }

        public override void OnDrawGizmos()
        {
        }

        private void Clear_ChaseGame()
        {
            // 게임 클리어 : CD 5개 다 모을 시 컷씬 진행 후 전환(다음 씬 서부로 전환)
        }

        private void Fail_ChaseGame()
        {
            // 게임 실패 : 얀데레한테 잡힐 시 컷씬 진행 후 복도 시작부터 다시 시작(재도전 UI 출력)
        }

        public void Create_Monster()
        {
            // 캐릭터 락
            m_player.Set_Lock(true);

            // 카메라 교체 및 설정
            CameraManager.Instance.Change_Camera(CAMERATYPE.CT_CUTSCENE);
            CameraCutscene camera = (CameraCutscene)CameraManager.Instance.Get_CurCamera();
            camera.Change_Position(new Vector3(0f, 1.2f, 20f));
            camera.Change_Rotation(new Vector3(0f, -180f, 25f));

            // 얀데레 생성
            m_yandereObj = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Character/Yandere"));
            m_yandere = m_yandereObj.GetComponent<HallwayYandere>();
            m_yandereTr = m_yandereObj.GetComponent<Transform>();
            m_yandereTr.position = new Vector3(0f, 0f, 2.8f);

            // 페이드 인
            UIManager.Instance.Start_FadeIn(1f, Color.black);
            // 돌면서 특정거리까지 줌인
            camera.Start_Cutscene(new Vector3(0f, 1.2f, 5.5f), new Vector3(0f, 180f, -16f), 2f, 0.5f);
            // 캐릭터가 말을 할때 얀데레 얼굴 클로즈업
            // 
        }

        private void Create_CD()
        {
            List<Vector3> beforePosition = new List<Vector3>();
            beforePosition.Add(new Vector3(0f, 0f, 0f));

            for (int i = 0; i < m_CdMaxCount; i++)
            {
                Vector3 newPosition = Get_RandomPositionOnNavMesh(beforePosition);
                Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Object/CD"), newPosition, Quaternion.identity);
                beforePosition.Add(newPosition);
            }
        }

        private void Create_Lever(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Vector3 NewPosition = Vector3.zero;
                while (true)
                {
                    NewPosition = VisualNovelManager.Instance.RandomPos[Random.Range(0, 20)].position;

                    bool Same = false;
                    for (int j = 0; j < m_Levers.Count; j++)
                    {
                        if (NewPosition == m_Levers[j].transform.position)
                            Same = true;
                    }

                    if (!Same)
                        break;
                }

                GameObject level = Instantiate(Resources.Load<GameObject>("5. Prefab/1. VisualNovel/Object/Lever"), NewPosition, Quaternion.identity);
                m_Levers.Add(level);
            }
        }

        public void Get_CD()
        {
            m_CdCurrentCount++;
            if (m_CdCurrentCount >= m_CdMaxCount)
            {
                // 추격 게임 종료
                Exit_Level();
            }
            else
            {
                // UI 업데이트
                VisualNovelManager.Instance.CdTxt.text = m_CdCurrentCount.ToString();

                // 조명 업데이트 Max 464
                m_Light.Shuffle();
                int OnCount = (int)(464 / (m_CdMaxCount - 1)) * m_CdCurrentCount;
                for (int i = 0; i < OnCount; ++i)
                    m_Light[i].Blink = true;

                // 대사 출력
            }

        }

        public void Use_Lever(GameObject self)
        {
            // 아이템 효과 적용
            if (m_yandereObj != null)
                m_yandere.Used_Lever();

            // 현재 아이템 삭제
            for (int i = 0; i < m_Levers.Count; i++)
            {
                if (self == m_Levers[i])
                {
                    m_Levers.RemoveAt(i);
                    Destroy(self);
                    break;
                }
            }

            // 추가 생성
            Create_Lever(1);
        }

        private Vector3 Get_RandomPositionOnNavMesh(List<Vector3> beforePos)
        {
            Vector3 position = new Vector3();
            bool select = false;

            int loopNum = 0;
            while (!select)
            {
                Vector3 randomPos = m_playerTr.position + Random.insideUnitSphere * m_CdMaxDistance; // 원하는 범위 내의 랜덤 방향 벡터 생성
                randomPos.y = 0.0f;
                NavMeshHit hit;

                // SamplePosition((Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
                // areaMask 에 해당하는 NavMesh 중에서 maxDistance 반경 내에서 sourcePosition에 가장 가까운 위치를 찾아서 그 결과를 hit에 담음
                if (NavMesh.SamplePosition(randomPos, out hit, m_CdMaxDistance, NavMesh.AllAreas)) // 위치 샘플링을 성공하면 참을 반환
                {
                    bool distMin = false;
                    foreach (Vector3 pos in beforePos)
                    {
                        float distX = Mathf.Abs(hit.position.x - pos.x);
                        float distZ = Mathf.Abs(hit.position.z - pos.z);
                        if (distX <= m_CdMinDistance || distZ <= m_CdMinDistance)
                            distMin = true;
                    }

                    if (!distMin)
                    {
                        position = hit.position;
                        select = true;
                    }
                }

                if (loopNum++ > 10000) // 고정 위치로 지정할 지 고민하기
                    throw new System.Exception("Infinite Loop");
            }

            return position;
        }
    }
}

