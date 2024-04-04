using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


public class DialogManager : MonoBehaviour
{
    private static DialogManager m_instance = null;
    public static DialogManager Instance
    {
        get //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 호출 가능
        {
            if (null == m_instance)
                return null;
            return m_instance;
        }
    }

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
    }

    public void Save_Data(string filePath, DialogData[] saveData)
    {
        var Result = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(filePath, Result);
    }

    public DialogData[] Load_Data(string filePath)
    {
        string Result = File.ReadAllText(filePath); // JSON 파일 읽기
        return JsonConvert.DeserializeObject<DialogData[]>(Result); // JSON 문자열을 DialogData 배열로 역직렬화
    }
}