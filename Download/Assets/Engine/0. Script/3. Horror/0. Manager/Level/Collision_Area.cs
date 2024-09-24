using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Area : MonoBehaviour
{
    [SerializeField] private int m_levelIndex;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false || GameManager.Ins.Horror.LevelController == null)
            return;

        Horror_Base stage = GameManager.Ins.Horror.LevelController.Get_CurrentLevel<Horror_Base>();
        if (stage == null)
            return;
        stage.Levels.Change_Level(m_levelIndex);
    }
}
