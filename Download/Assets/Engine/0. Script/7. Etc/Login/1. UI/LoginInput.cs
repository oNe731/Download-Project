using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginInput : MonoBehaviour
{
    private TMP_InputField m_nameInput;
    private int m_maxLength = 5;
    private bool m_click = false;

    private void Start()
    {
        m_nameInput = transform.GetChild(2).GetComponent<TMP_InputField>();
        m_nameInput.characterLimit = m_maxLength;
    }

    private void Update()
    {
        if (GameManager.Ins.IsGame == false)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Button_InputName();
        }
    }

    public void Button_InputName()
    {
        if (m_nameInput.text.Length == 0 || m_click == true)
            return;

        m_click = true;
        StartCoroutine(PlaySoundAndChangeScene());
    }

    private IEnumerator PlaySoundAndChangeScene()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        GameManager.Ins.PlayerName = m_nameInput.text;
        GameManager.Ins.Change_Scene(StageManager.STAGE.LEVEL_WINDOW);
    }

    public void Button_Exit()
    {
        GameManager.Ins.End_Game();
    }
}
