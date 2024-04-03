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

    [Header("SaveData")]
    [SerializeField] private string m_filePath;
    [SerializeField] private DialogData[] m_saveData;

    private DialogData[] m_loadData;

    private void Awake()
    {
        if (null == m_instance)
            m_instance = this;
    }

    private void Start()
    {
        //Save_Data(FilePath);
    }

    public void Create_Dialog(string filePath)
    {
        //GameObject Clone = Instantiate(Prefab_Dialog);
        //if (Clone)
        //{
        //    Dialog DialogCom = Clone.GetComponent<Dialog>();
        //    if (DialogCom)
        //    {
        //        Load_Data(filePath);
        //        DialogCom.Dialogs = LoadData;
        //    }
        //}
    }

    private void Save_Data(string filePath)
    {
        var Result = JsonConvert.SerializeObject(m_saveData);
        File.WriteAllText(filePath, Result);

        Debug.Log("JSON 파일 저장 : " + filePath);
    }

    private void Load_Data(string filePath)
    {
        string Result = File.ReadAllText(filePath); // JSON 파일 읽기
        m_loadData = JsonConvert.DeserializeObject<DialogData[]>(Result); // JSON 문자열을 DialogData 배열로 역직렬화

        Debug.Log("JSON 파일 불러오기 : " + filePath);
    }
}