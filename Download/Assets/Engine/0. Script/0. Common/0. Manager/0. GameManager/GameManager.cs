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
        TextAsset jsonAsset = Resources.Load<TextAsset>(filePath);

        if (jsonAsset != null)
            return JsonConvert.DeserializeObject<List<T>>(jsonAsset.text);
        else
            Debug.LogError($"Failed to load JSON data : {filePath}");

        return null;
    }

    public GameObject Create_GameObject(string path, Transform transform)
    {
        return Instantiate(Resources.Load<GameObject>(path), transform);
    }
}