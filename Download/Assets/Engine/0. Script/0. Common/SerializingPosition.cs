using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PositionData
{
    public List<Vector3> positions;
}

public class SerializingPosition : MonoBehaviour
{
    [SerializeField] private string     m_path = "Assets/Resources/4. Data/1. VisualNovel/Position/ItemPositionData";
    [SerializeField] private GameObject m_prefab;

    private void Update()
    {
#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    SavePositions();
        //}
        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadPositions();
        }
#endif
    }

    private void SavePositions()
    {
        PositionData data = new PositionData();
        data.positions = new List<Vector3>();

        foreach (Transform child in transform) { data.positions.Add(child.position); }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(m_path, json);

        Debug.Log("Positions saved to " + m_path);
    }

    private void LoadPositions()
    {
        if (File.Exists(m_path))
        {
            string json = File.ReadAllText(m_path);
            PositionData data = JsonUtility.FromJson<PositionData>(json);

            foreach (Transform child in transform) { Destroy(child.gameObject); }
            foreach (Vector3 position in data.positions) { Instantiate(m_prefab, position, Quaternion.identity, transform); }

            Debug.Log("Positions loaded to " + m_path);
        }
    }
}