using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager Instance => m_instance;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject); //씬 전환이 되더라도 파괴되지 않음
        }
        else
            Destroy(this.gameObject); //이미 전역변수인 instance에 인스턴스가 존재한다면 자신을 삭제
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void Save_JsonData<T>(string filePath, List<T> saveData)
    {
        var Result = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(filePath, Result);
    }

    public List<T> Load_JsonData<T>(string filePath)
    {
        string Result = File.ReadAllText(filePath);        // JSON 파일 읽기
        return JsonConvert.DeserializeObject<List<T>>(Result); // JSON 문자열을 제너릭 타입 배열로 역직렬화
    }
}