using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> m_bgm = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> m_effect = new Dictionary<string, AudioClip>();

    private void Start()
    {
        Load_Resource();
    }

    private void Load_Resource()
    {
        #region 공포 게임 사운드

        #region 플레이어
        m_effect.Add("Horror_Player_Walk", GameManager.Ins.Resource.Load<AudioClip>("2. Sound/3. Horror/Effect/Player/Walk/Walk"));

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
        audioSource.Stop();

        audioSource.clip = m_effect[name];
        audioSource.loop = loop;
        audioSource.pitch = speed; // 기본1f
        audioSource.Play();
    }

    public void Play_AudioSource(ref AudioSource audioSource, string name, bool loop, float speed)
    {
        audioSource.Stop();

        audioSource.clip  = m_effect[name];
        audioSource.loop  = loop;
        audioSource.pitch = speed; // 기본1f
        audioSource.Play();
    }

    public void Stop_AudioSource(ref AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void Change_Bgm()
    {

    }
}
