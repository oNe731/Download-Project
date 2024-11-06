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

    public string cutSceneEvents;
    public string imageName;

    public int gameType;

    public bool nextIndex;
    //public bool usePosition;
    //public Vector3 targetPosition;
    //public float positionSpeed;
    //public bool useRotation;
    //public Vector3 targetRotation;
    //public float rotationSpeed;
}
