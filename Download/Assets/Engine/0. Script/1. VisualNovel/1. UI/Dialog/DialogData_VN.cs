using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VisualNovel
{
    #region 기본 다이얼로그 데이터
    [Serializable]
    public class DialogData_VN
    {
        public enum DIALOG_TYPE
        {
            DT_FADE,      // 페이드
            DT_DIALOG,    // 다이얼로그 + 선택지 + 호감도
            DT_GAMESTATE, // 게임 상태 변경
            DT_CUTSCENE,  // 컷씬 및 카메라/ 쉐이킹
        }

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

#region 세부 다이얼로그 데이터
    public interface DialogSubData
    {
    }

#region 페이드 데이터
    [Serializable]
    public struct FadeData : DialogSubData
    {
        public enum FADETYPE { FT_IN, FT_OUT, FT_INOUT, FT_OUTIN, FT_NONE, FT_END }

        public FADETYPE fadeType;
        public int pathIndex;
    }
#endregion

#region 기본 다이얼로그 데이터
    [Serializable]
    public struct DialogData : DialogSubData
    {
        public VisualNovelManager.OWNERTYPE owner; // 오너 타입
        public string dialogName;                  // 이름
        public string dialogText;                  // 대사

        public string backgroundSpr;     // 배경
        public List<string> standingSpr; // 스탠딩

        // 기타 옵션
        public int eventIndex;
        public int addLike;           // 호감도 증가
        public ChoiceData choiceData; // 선택지 버튼
    }

    public struct ChoiceData
    {
        public enum CHOICETYPE
        {
            CT_CLOSE,  // 닫기      : 0
            CT_DIALOG, // 다이얼로그 : 1

            CT_END
        };

        public bool choiceLoop;
        public List<CHOICETYPE> choiceEventType;
        public List<string> choiceText;
        public List<int> choiceDialog;
        public int pathIndex;
    }
#endregion

#region 게임 상태 데이터
    [Serializable]
    public struct GameState : DialogSubData
    {
        public enum GAMETYPE { GT_DAY1, GT_DAY2, GT_DAY3, GT_STARTSHOOT, GT_STARTCHASE, GT_PLAYCHASE, GT_END }

        public GAMETYPE gameType;
    }
#endregion

#region 게임 컷씬 데이터
    [Serializable]
    public struct CutScene : DialogSubData
    {
        public enum CUTSCENETYPE
        {
            // 깜빡거림, 카메라조작, 애니메이션변경, 패널이벤트, 활성화변경, 이미지컷씬, 카메라 쉐이킹
            CT_BLINK, CT_CAMERA, CT_ANIMATION, CT_LIKEPANEL, CT_ACTIVE, CT_IMAGE, CT_SHAKE,
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
            else if (jsonObject["imageName"] != null)
                return jsonObject.ToObject<ImageValue>();
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

        public VisualNovelManager.OWNERTYPE owner; // 오너 타입
        public string dialogName;                  // 이름
        public string dialogText;                  // 대사
        public string animatroTriger;              // 애니메이션 트리거 이름
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

    [Serializable]
    public struct ImageValue : CutSceneValue
    {
        public string imageName;
    }
#endregion
#endregion
}

