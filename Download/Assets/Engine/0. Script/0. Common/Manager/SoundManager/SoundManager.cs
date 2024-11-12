using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SOUNDTYPE { TYPE_BGM, TYPE_EFFECT };

    private Dictionary<string, AudioClip> m_bgm = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> m_effect = new Dictionary<string, AudioClip>();

    private float m_bgmSound    = 0.2f;
    private float m_effectSound = 0.5f;

    public float BgmSound { get => m_bgmSound; set => m_bgmSound = value; }
    public float EffectSound { get => m_effectSound; set => m_effectSound = value; }

    private void Start()
    {
        Load_Resource();
    }

    public void Update_AllAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in allAudioSources)
        {
            if (audioSource.gameObject.CompareTag("MainCamera") == true)
                audioSource.volume = m_bgmSound;
            else
                audioSource.volume = m_effectSound;
        }
    }

    private void Load_Resource()
    {
        #region 윈도우 게임 사운드
        m_effect.Add("Window_Login", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_02_Login"));
        m_effect.Add("Window_SiteBgm", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_04_SiteBgm"));

        m_effect.Add("Window_Click", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_03_Click"));
        m_effect.Add("Window_MascotSpeak", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_21_Mascot_Speak"));

        m_effect.Add("Window_DeleteHit", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_10_DeleteGame&Hit"));
        m_effect.Add("Window_Knock", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_11_Novel_Knock"));
        m_effect.Add("Window_broken", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_12_Novel_broken"));
        m_effect.Add("Window_Golfhit", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_14_Ayaka_Golf_hit"));
        m_effect.Add("Window_Golfdrop", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/0. Window/Window_15_Ayaka_Golf_drop"));
        #endregion

        #region 미연시 게임 사운드
        #region BGM
        m_bgm.Add("VisualNovel_ScriptBGM",        GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/Script"));
        m_bgm.Add("VisualNovel_ShootBGM",         GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/Shoot"));
        m_bgm.Add("VisualNovel_CellarBGM",        GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/Cellar"));
        m_bgm.Add("VisualNovel_YandereAppearBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/YandereAppear"));
        m_bgm.Add("VisualNovel_ChaseBGM",         GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/BGM/Chase"));
        #endregion

        #region Effect
        m_effect.Add("VisualNovel_Charging",      GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ShotGame/Charging"));
        m_effect.Add("VisualNovel_Shoot",         GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ShotGame/Shoot"));
        m_effect.Add("VisualNovel_AttackSuccess", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ShotGame/AttackSuccess"));
        m_effect.Add("VisualNovel_TimeOver",      GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ShotGame/TimeOver"));
        m_effect.Add("VisualNovel_DollsExploded", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ShotGame/DollsExploded"));

        m_effect.Add("VisualNovel_YandereSmile", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ChaseGame/Yandere/Effect/YandereSmile"));

        // 플레이어
        m_effect.Add("VisualNovel_Player_Steps_0", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ChaseGame/PlayerSteps/Steps1"));
        m_effect.Add("VisualNovel_Player_Steps_1", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ChaseGame/PlayerSteps/Steps2"));
        m_effect.Add("VisualNovel_Player_Steps_2", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ChaseGame/PlayerSteps/Steps3"));
        m_effect.Add("VisualNovel_Player_Steps_3", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ChaseGame/PlayerSteps/Steps4"));
        m_effect.Add("VisualNovel_Player_Steps_4", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/1. VisualNovel/Effect/ChaseGame/PlayerSteps/Steps5"));
        #endregion
        #endregion

        #region 서부 게임 사운드
        #region BGM
        m_bgm.Add("Western_StartBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/Western_1_BGM_Start"));
        m_bgm.Add("Western_MainBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/Main"));
        m_bgm.Add("Western_WantedBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/Wanted"));
        m_bgm.Add("Western_Play1BGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/Play1"));
        m_bgm.Add("Western_BarBGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/BGM/La Docerola - Quincas Moreira"));
        m_bgm.Add("Western_Play2BGM", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/BGM/Silencios de Los Angeles - Cumbia Deli")); //
        #endregion

        #region Effect
        m_effect.Add("Western_Gun_Attacked",    GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/UI/Gun/GunHit"));
        m_effect.Add("Western_Gun_Shoot",       GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/UI/Gun/GunShoot"));
        m_effect.Add("Western_Panel_Rotation",  GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/GameObject/PanelRotation"));
        m_effect.Add("Western_Criminal_Caught", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/GameObject/CriminalCaught"));
        m_effect.Add("Western_Clear_Dialogue",  GameManager.Ins.Resource.Load<AudioClip>("2. Sound/2. Western/Effect/UI/Clear/ClearDialogue"));
        #endregion
        #endregion

        #region 공포 게임 사운드
        #region Effect
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
        m_effect.Add("Horror_Bug_Attack", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Monster/Bug/Worm_Attack"));
        m_effect.Add("Horror_Straitjacket_Idle", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Monster/Straitjacket/Zombie_Notice_3"));
        m_effect.Add("Horror_Straitjacket_Attack", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Monster/Straitjacket/Zombie_Attack"));
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
        #endregion
    }

    public void Play_ManagerAudioSource(string name, bool loop, float speed)
    {
        AudioSource audioSource = GameManager.Ins.AudioSource;
        Play_AudioSource(audioSource, name, loop, speed);
    }

    public void Play_AudioSource(AudioSource audioSource, string name, bool loop, float speed)
    {
        Play_AudioSource(audioSource, m_effect[name], loop, speed, m_effectSound);
    }

    public void Play_AudioSourceBGM(string name, bool loop, float speed)
    {
        Play_AudioSource(Camera.main.GetComponent<AudioSource>(), m_bgm[name], loop, speed, m_bgmSound);
    }

    public void Stop_AudioSourceBGM()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
    }

    public void Play_AudioSource(AudioSource audioSource, AudioClip audioClip, bool loop, float speed, float volume)
    {
        audioSource.Stop();

        audioSource.clip   = audioClip;
        audioSource.loop   = loop;
        audioSource.pitch  = speed; // 기본1f
        audioSource.volume = volume;
        audioSource.Play();
    }

    public AudioClip Get_EffectAudioClip(string name)
    {
        return m_effect[name];
    }

    public void Stop_AudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void Change_Bgm()
    {

    }
}
