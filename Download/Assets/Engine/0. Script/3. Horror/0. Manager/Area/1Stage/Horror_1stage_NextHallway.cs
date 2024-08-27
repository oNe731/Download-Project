using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horror_1stage_NextHallway : Area
{
    [SerializeField] private Dummy m_dummy;
    [SerializeField] private GameObject m_monsterTriger;
    private bool m_event = false;

    public override void Initialize_Level(LevelController levelController)
    {
        base.Initialize_Level(levelController);

        m_levelIndex = (int)Horror_1stage.LEVEL1.LV_NEXTHALLWAY;
    }

    public override bool Check_Clear(Interaction_Door interaction_Door, ref float[] activeTimes, ref string[] texts)
    {
        // D 
        if(interaction_Door.DoorIndex == 4)
        {
            // 열쇠가 있는가?
            Horror.Note note = HorrorManager.Instance.Player.Note;
            if (note != null)
            {
                if (note.Check_Item(NoteItem.ITEMTYPE.TYPE_1KEY))
                    return true;
            }

            activeTimes = new float[1];
            texts = new string[1];
            activeTimes[0] = 1f;
            texts[0] = "잠겨있다. 열쇠가 필요해 보인다.";

            // 이벤트 발생
            if(m_event == false)
            {
                m_event = true;
                StartCoroutine(Event_Door(texts[0]));
            }
        }
        // H
        else if(interaction_Door.DoorIndex == 8)
        {
            activeTimes = new float[1];
            texts = new string[1];
            activeTimes[0] = 1f;
            texts[0] = "열리지 않는다. 반대쪽에서 열 수 있을 것 같은 구조다.";
        }

        return false;
    }


    public override void Enter_Level()
    {
    }

    public override void Play_Level()
    {
    }

    public override void Update_Level()
    {
    }

    public override void LateUpdate_Level()
    {
    }

    public override void Exit_Level()
    {
    }

    public override void OnDrawGizmos()
    {
    }

    private IEnumerator Event_Door(string text)
    {
#region 안내 문구가 끝났다면 이벤트 발생
        UIInstruction instruction = HorrorManager.Instance.InstructionUI;
        while (true)
        {
            if (instruction.Texts.Length != 0)
            {
                if (instruction.Texts[0] == text)
                {
                    if (instruction.Active == true && instruction.gameObject.activeSelf == false)
                        break;
                }
            }
            yield return null;
        }
        #endregion

#region 카메라 쉐이킹        
        HorrorManager.Instance.Set_Pause(true); // 게임 일시정지

        CameraFollow camera = (CameraFollow)GameManager.Instance.Camera.Get_CurCamera();
        camera.Start_Shake(3f, 0.5f);

        while (true)
        {
            if (camera == null || camera.IsShake == false)
                break;
            yield return null;
        }
        
        HorrorManager.Instance.Set_Pause(false); // 게임 일시정지 해제
        #endregion

        m_dummy.Fall_Dummy(); // 더미 오브젝트 이벤트 발생.

#region 몹 생성 (구속복/ 애벌레)
        m_monsterTriger.SetActive(true);
#endregion

#region 맵 요소 생성 및 변경 (추후 작업)
        // 분홍색 벽 부분에 피로 적힌 글씨가 생긴다.
        // 빗금표시(////)되어있는 곳에 발자국(발없는 보스로 바뀌면서 다른 흔적으로 변경 예상)이 생긴다.
#endregion

        yield break;
    }
}
