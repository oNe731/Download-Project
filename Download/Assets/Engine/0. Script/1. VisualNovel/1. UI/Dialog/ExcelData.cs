using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExcelData
{
    public int dialogType;

    public int fadeType;
    public int pathIndex;

    public int owner;
    public string dialogName;
    public string dialogText;
    public string backgroundSpr;
    public string standingSpr;
    public int addLike;
    public bool choiceLoop;
    public string choiceEventType;
    public string choiceText;
    public string choiceDialog;
    public int gameType;

    // -----
    public string cutSceneEvents;
    public bool nextIndex;
    public bool usePosition;
    public string targetPosition;
    public float positionSpeed;
    public bool useRotation;
    public string targetRotation;
    public float rotationSpeed;
    public string animatroTriger;
    public int objectType;
    public bool active;
    public string imageName;
}