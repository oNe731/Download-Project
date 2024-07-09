using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VisualNovel
{
    [Serializable]
    public class DialogData_VN
    {
        public enum DIALOGEVENT_TYPE
        {
            DET_NONE,                                              // 기본 다이얼로그 : 0
            DET_FADEIN, DET_FADEOUT, DET_FADEINOUT, DET_FADEOUTIN, // 페이드         : 1 2 3 4
            DET_STARTSHOOT, DET_STARTCHASE, DET_PLAYCHASE,         // 게임 변경      : 5 6 7
            DET_LIKEADD, DET_SHAKING, DET_CUTSCENE,                // 기타 효과      : 8 9 10

            DET_END
        };

        public enum CHOICEEVENT_TYPE
        {
            CET_CLOSE,  // 0
            CET_DIALOG, // 1

            CET_END
        };

        public DIALOGEVENT_TYPE dialogEvent;

        public VisualNovelManager.NPCTYPE owner;
        public string nameText;
        public string dialogText;

        // 리소스 관련
        public string backgroundSpr;
        public List<string> standingSpr;
        public string portraitSpr;
        public string boxSpr;
        public string ellipseSpr;
        public string arrawSpr;
        public string nameFont;
        public string dialogFont;

        // 선택지 관련
        public List<CHOICEEVENT_TYPE> choiceEventType;
        public List<string> choiceText;
        public List<string> choiceDialog;

        // 컨씬 관련
        public DialogCutScene dialogCutScene;
    }


    [Serializable]
    public class DialogCutScene
    {
        public enum CUTSCENEEVENT_TYPE
        {
            CET_BLINK, CET_CAMERA, CET_ANIMATION, CET_LIKEPANEL, CET_ACTIVE,
            CET_END
        };

        public List<CUTSCENEEVENT_TYPE> cutSceneEvents;

        [JsonProperty(ItemConverterType = typeof(EventValueConverter))]
        public List<EventValue> eventValues;
    }

    public interface EventValue
    {
    }

    [Serializable]
    public struct BasicValue : EventValue
    {
        public bool nextIndex;
    }

    [Serializable]
    public struct CameraValue : EventValue
    {
        public bool nextIndex;

        public bool    usePosition;
        public Vector3 targetPosition;
        public float   positionSpeed;

        public bool    useRotation;
        public Vector3 targetRotation;
        public float   rotationSpeed;
    }

    [Serializable]
    public struct AnimationValue : EventValue
    {
        public enum OBJECT_TYPE
        {
            OJ_YANDERE,
            OJ_END
        };

        public bool nextIndex;
        public OBJECT_TYPE objectType;
        public string animatroTriger;
    }

    [Serializable]
    public struct ActiveValue : EventValue
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

    public class EventValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(EventValue).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            if (jsonObject["usePosition"] != null)
                return jsonObject.ToObject<CameraValue>();
            else if (jsonObject["active"] != null)
                return jsonObject.ToObject<ActiveValue>();
            else if (jsonObject["objectType"] != null)
                return jsonObject.ToObject<AnimationValue>();
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
}

