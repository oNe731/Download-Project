using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VisualNovel
{
    #region �⺻ ���̾�α� ������
    [Serializable]
    public class DialogData_VN
    {
        public enum DIALOG_TYPE
        {
            DT_FADE,      // ���̵�
            DT_DIALOG,    // ���̾�α� + ������ + ȣ����
            DT_GAMESTATE, // ���� ���� ����
            DT_CUTSCENE,  // �ƾ� �� ī�޶�/ ����ŷ
        }

        public string bgm;
        public string effect;

        public DIALOG_TYPE dialogType;

        [JsonConverter(typeof(DialogSubDataConverter))]
        public DialogSubData dialogSubData;
    }

    public class DialogSubDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DialogSubData).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            if (jsonObject["fadeType"] != null)
            {
                return jsonObject.ToObject<FadeData>();
            }
            else if (jsonObject["owner"] != null)
            {
                return jsonObject.ToObject<DialogData>();
            }
            else if (jsonObject["gameType"] != null)
            {
                return jsonObject.ToObject<GameState>();
            }
            else if (jsonObject["cutSceneEvents"] != null)
            {
                return jsonObject.ToObject<CutScene>();
            }

            throw new JsonSerializationException("Unknown DialogSubDataType");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
#endregion

#region ���� ���̾�α� ������
    public interface DialogSubData
    {
    }

#region ���̵� ������
    [Serializable]
    public struct FadeData : DialogSubData
    {
        public enum FADETYPE { FT_IN, FT_OUT, FT_INOUT, FT_OUTIN, FT_NONE, FT_END }

        public FADETYPE fadeType;
        public int pathIndex;
    }
#endregion

#region �⺻ ���̾�α� ������
    [Serializable]
    public struct DialogData : DialogSubData
    {
        public VisualNovelManager.OWNERTYPE owner; // ���� Ÿ��
        public string dialogName;                  // �̸�
        public string dialogText;                  // ���

        public string backgroundSpr;     // ���
        public List<string> standingSpr; // ���ĵ�

        // ��Ÿ �ɼ�
        public int eventIndex;
        public int addLike;           // ȣ���� ����
        public ChoiceData choiceData; // ������ ��ư
    }

    public struct ChoiceData
    {
        public enum CHOICETYPE
        {
            CT_CLOSE,  // �ݱ�      : 0
            CT_DIALOG, // ���̾�α� : 1

            CT_END
        };

        public bool choiceLoop;
        public List<CHOICETYPE> choiceEventType;
        public List<string> choiceText;
        public List<int> choiceDialog;
        public int pathIndex;
    }
#endregion

#region ���� ���� ������
    [Serializable]
    public struct GameState : DialogSubData
    {
        public enum GAMETYPE { GT_DAY1, GT_DAY2, GT_DAY3, GT_STARTSHOOT, GT_STARTCHASE, GT_PLAYCHASE, GT_END }

        public GAMETYPE gameType;
    }
#endregion

#region ���� �ƾ� ������
    [Serializable]
    public struct CutScene : DialogSubData
    {
        public enum CUTSCENETYPE
        {
            // �����Ÿ�, ī�޶�����, �ִϸ��̼Ǻ���, �г��̺�Ʈ, Ȱ��ȭ����
            CT_BLINK, CT_CAMERA, CT_ANIMATION, CT_LIKEPANEL, CT_ACTIVE,

            // ȣ���� ó��, ���ݱ�, �᷹�� ������ �ȱ�
            CT_LIKEDIALOG, CT_CLOSEBACK, CT_YANWALK,
            CT_END
        };

        public List<CUTSCENETYPE> cutSceneEvents;

        [JsonProperty(ItemConverterType = typeof(CutSceneValueConverter))]
        public List<CutSceneValue> eventValues;
    }

    public class CutSceneValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(CutSceneValue).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            if (jsonObject["usePosition"] != null)
                return jsonObject.ToObject<CameraValue>();
            else if (jsonObject["animatroTriger"] != null)
                return jsonObject.ToObject<AnimationValue>();
            else if (jsonObject["active"] != null)
                return jsonObject.ToObject<ActiveValue>();
            else if (jsonObject["nextIndex"] != null)
                return jsonObject.ToObject<BasicValue>();

            throw new JsonSerializationException("Unknown type of EventValue");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is CameraValue cameraValue)
                JObject.FromObject(cameraValue).WriteTo(writer);
            else if (value is AnimationValue animationValue)
                JObject.FromObject(animationValue).WriteTo(writer);
            else if (value is ActiveValue activeValue)
                JObject.FromObject(activeValue).WriteTo(writer);
            else if (value is BasicValue basicValue)
                JObject.FromObject(basicValue).WriteTo(writer);
            else
                throw new JsonSerializationException("Unsupported type of EventValue");
        }
    }

    public interface CutSceneValue
    {
    }

    [Serializable]
    public struct BasicValue : CutSceneValue
    {
        public bool nextIndex;
    }

    [Serializable]
    public struct CameraValue : CutSceneValue
    {
        public bool nextIndex;

        public bool usePosition;
        public Vector3 targetPosition;
        public float positionSpeed;

        public bool useRotation;
        public Vector3 targetRotation;
        public float rotationSpeed;
    }

    [Serializable]
    public struct AnimationValue : CutSceneValue
    {
        public bool nextIndex;

        public string dialogName;                  // �̸�
        public string dialogText;                  // ���
        public string animatroTriger;              // �ִϸ��̼� Ʈ���� �̸�
    }

    [Serializable]
    public struct ActiveValue : CutSceneValue
    {
        public enum OBJECT_TYPE
        {
            OJ_SAW,
            OJ_END
        };

        public bool nextIndex;
        public OBJECT_TYPE objectType;
        public bool active;
    }
#endregion
#endregion
}

