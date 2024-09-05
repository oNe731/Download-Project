using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SOUNDTYPE { TYPE_BGM, TYPE_EFFECT };

    private Dictionary<string, AudioClip> m_bgm = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> m_effect = new Dictionary<string, AudioClip>();

    private void Start()
    {
        Load_Resource();
    }

    private void Load_Resource()
    {
        #region 미연시 게임 사운드
        m_bgm.Add("VisualNovel_ShootBGM",  GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/SS"));
        m_bgm.Add("VisualNovel_CellarBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/지하실 BGM"));
        m_bgm.Add("VisualNovel_ChaseBGM",  GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/추격게임 BGM"));
        #endregion

        #region 서부 게임 사운드
        m_bgm.Add("Western_MainBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/메인화면 BGM"));
        m_bgm.Add("Western_WantedBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/수배지 BGM"));
        m_bgm.Add("Western_TutorialBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/튜토리얼 BGM"));
        m_bgm.Add("Western_TutorialAfterBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/튜토리얼 이후 BGM"));
        m_bgm.Add("Western_PlayBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/BGM/Silencios de Los Angeles - Cumbia Deli"));
        m_bgm.Add("Western_BarBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/La Docerola - Quincas Moreira2"));

        m_effect.Add("Western_Gun_Attacked", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/UI/에임 맞췄을 때"));
        m_effect.Add("Western_Gun_Shoot", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/UI/총소리"));
        #endregion

        #region 공포 게임 사운드

        #region 플레이어
        m_effect.Add("Horror_Player_Walk_0", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_0"));
        m_effect.Add("Horror_Player_Walk_1", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_1"));
        m_effect.Add("Horror_Player_Walk_2", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_2"));
        m_effect.Add("Horror_Player_Walk_3", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_3"));
        m_effect.Add("Horror_Player_Walk_4", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_4"));
        m_effect.Add("Horror_Player_Walk_5", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_5"));
        m_effect.Add("Horror_Player_Walk_6", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_6"));
        m_effect.Add("Horror_Player_Walk_7", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_7"));
        m_effect.Add("Horror_Player_Walk_8", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_8"));
        m_effect.Add("Horror_Player_Walk_9", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_9"));
        m_effect.Add("Horror_Player_Walk_10", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_10"));
        m_effect.Add("Horror_Player_Walk_11", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_11"));
        m_effect.Add("Horror_Player_Walk_12", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_12"));
        m_effect.Add("Horror_Player_Walk_13", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_13"));
        m_effect.Add("Horror_Player_Walk_14", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_14"));
        m_effect.Add("Horror_Player_Walk_15", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_15"));
        m_effect.Add("Horror_Player_Walk_16", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_16"));
        m_effect.Add("Horror_Player_Walk_17", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk_17"));

        // 무기
        m_effect.Add("Horror_Weapon_Bbaru_Install", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Weapon/Bbaru/Bbaru_Install/Stick_Wearing"));
        m_effect.Add("Horror_Weapon_Bbaru_Attack", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Weapon/Bbaru/Bbaru_Attack/Stick_Swing_2"));
        m_effect.Add("Horror_Weapon_Bbaru_Damaged", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Weapon/Bbaru/Bbaru_Damaged/Stick_Attack_1"));

        m_effect.Add("Horror_Weapon_Gun_Install", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Weapon/Gun/Gun_Install/Gun_Wearing"));
        m_effect.Add("Horror_Weapon_Gun_Attack", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Weapon/Gun/Gun_Attack/Gun_Shot"));

        m_effect.Add("Horror_Weapon_Handlight_Install", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Weapon/Handlight/Handlight_Install/Flashlight_Wearing"));

        // 기타
        m_effect.Add("Horror_ClickInteraction", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Etc/ClickInteraction/Click_Interaction"));
        m_effect.Add("Horror_GetItem", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Etc/GetItem/Get_Item"));
        m_effect.Add("Horror_OpenNote", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Etc/OpenNote/BookUI_Open"));
        #endregion

        #region 몬스터
        m_effect.Add("Horror_Bug_Fly", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Monster/Bug/Worm_Flight"));
        m_effect.Add("Horror_Straitjacket_Idle", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Monster/Straitjacket/Zombie_Notice_3"));
        #endregion

        #region 오브젝트
        m_effect.Add("Horror_Open_Door", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Object/Door/Open_Door"));

        m_effect.Add("Horror_Dummy1", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Object/Dummy/Delete_Varigite_1"));
        m_effect.Add("Horror_Dummy2", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Object/Dummy/Delete_Varigite_2"));
        #endregion

        #region 기타
        m_effect.Add("FirstHallway", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Etc/FirstHallway/First_Hallway_1"));
        m_effect.Add("DummyHallway", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Etc/DummyHallway/Delete_Varigite_Hallway"));
        #endregion

        #endregion
    }

    public void Play_ManagerAudioSource(string name, bool loop, float speed)
    {
        AudioSource audioSource = GameManager.Ins.AudioSource;
        Play_AudioSource(audioSource, name, loop, speed);
    }

    public void Play_AudioSource(AudioSource audioSource, string name, bool loop, float speed)
    {
        audioSource.Stop();

        audioSource.clip  = m_effect[name];
        audioSource.loop  = loop;
        audioSource.pitch = speed; // 기본1f
        audioSource.Play();
    }

    public void Play_AudioSourceBGM(string name, bool loop, float speed)
    {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        audioSource.Stop();

        audioSource.clip = m_bgm[name];
        audioSource.loop = loop;
        audioSource.pitch = speed; // 기본1f
        audioSource.Play();
    }

    public void Stop_AudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void Change_Bgm()
    {

    }
}
