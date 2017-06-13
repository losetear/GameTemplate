using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using XLua;
using Object = UnityEngine.Object;

namespace GameTemplate
{
    public static class LuaConfig
    {
        [CSharpCallLua] 
        public static List<Type> CSharpCallLua = new List<Type>
        {
            typeof (Action),
            typeof (Action<GameObject>),
            typeof (Action<int, GameObject>),
            typeof (Action<AudioClip>),
            typeof (UnityEngine.Hash128),
            typeof (Action<bool>),
            typeof (SocketConnected),
        };

        private static readonly List<Type> CustomTypes = new List<Type>();

        [BlackList] 
        public static List<List<string>> BlackList = new List<List<string>>
        {
            new List<string> {"UnityEngine.Light", "lightmappingMode"},
            new List<string> {"UnityEngine.UI.Graphic", "OnRebuildRequested"},
            new List<string> {"UnityEngine.UI.Text", "OnRebuildRequested"},
            new List<string> {"UnityEngine.AnimatorControllerParameter", "name"},
            new List<string> {"UnityEngine.Caching", "SetNoBackupFlag","System.String","System.Int32"},
            new List<string> {"UnityEngine.Caching", "SetNoBackupFlag","System.String","UnityEngine.Hash128"},
            new List<string> {"UnityEngine.Caching", "ResetNoBackupFlag","System.String","System.Int32"},
            new List<string> {"UnityEngine.Caching", "ResetNoBackupFlag","System.String","UnityEngine.Hash128"},
            new List<string> {"UnityEngine.Input", "IsJoystickPreconfigured","System.String"},
        };

        private static readonly List<string> Exclude = new List<string>
        {
            "GraphicRebuildTracker",
            "HideInInspector",
            "ExecuteInEditMode",
            "AddComponentMenu",
            "ContextMenu",
            "RequireComponent",
            "DisallowMultipleComponent",
            "SerializeField",
            "AssemblyIsEditorAssembly",
            "Attribute",
            "Types",
            "UnitySurrogateSelector",
            "TrackedReference",
            "TypeInferenceRules",
            "FFTWindow",
            "RPC",
            "Network",
            "MasterServer",
            "BitStream",
            "HostData",
            "ConnectionTesterStatus",
            "GUI",
            "EventType",
            "EventModifiers",
            "FontStyle",
            "TextAlignment",
            "TextEditor",
            "TextEditorDblClickSnapping",
            "TextGenerator",
            "TextClipping",
            "Gizmos",
            "ADBannerView",
            "ADInterstitialAd",
            "Android",
            "Tizen",
            "jvalue",
            "iPhone",
            "iOS",
            "Windows",
            "CalendarIdentifier",
            "CalendarUnit",
            "CalendarUnit",
            "ClusterInput",
            "FullScreenMovieControlMode",
            "FullScreenMovieScalingMode",
            "Handheld",
            "LocalNotification",
            "NotificationServices",
            "RemoteNotificationType",
            "RemoteNotification",
            "SamsungTV",
            "TextureCompressionQuality",
            "TouchScreenKeyboardType",
            "TouchScreenKeyboard",
            "MovieTexture",
            "UnityEngineInternal",
            "Terrain",
            "Tree",
            "SplatPrototype",
            "DetailPrototype",
            "DetailRenderMode",
            "MeshSubsetCombineUtility",
            "AOT",
            "Social",
            "Enumerator",
            "SendMouseEvents",
            "Cursor",
            "Flash",
            "ActionScript",
            "OnRequestRebuild",
            "Ping",
            "ShaderVariantCollection",
            "SimpleJson.Reflection",
            "CoroutineTween",
            "GraphicRebuildTracker",
            "Advertisements",
            "UnityEditor",
            "WSA",
            "EventProvider",
            "Apple",
            "ClusterInput",
            "Motion",
            "UnityEngine.UI.ReflectionMethodsCache"
        };

        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp_Common
        {
            get
            {
                var list = new List<Type>
                {
                    typeof (Light),
                    typeof (LightmapBakeType),
                    typeof (object),
                    typeof (Object),
                    typeof (Vector2),
                    typeof (Vector3),
                    typeof (Vector4),
                    typeof (Quaternion),
                    typeof (Color),
                    typeof (Ray),
                    typeof (Bounds),
                    typeof (Ray2D),
                    typeof (Time),
                    typeof (GameObject),
                    typeof (Component),
                    typeof (Behaviour),
                    typeof (Transform),
                    typeof (Resources),
                    typeof (TextAsset),
                    typeof (Keyframe),
                    typeof (AnimationCurve),
                    typeof (AnimationClip),
                    typeof (MonoBehaviour),
                    typeof (CombineInstance),
                    typeof (ParticleSystem),
                    typeof (SkinnedMeshRenderer),
                    typeof (Renderer),
                    typeof (WWW),
                    typeof (Application),
                    typeof (Physics),
                    typeof (MeshFilter),
                    typeof (MeshRenderer),
                    typeof (Rigidbody),
                    typeof (GUITexture),
                    typeof (Animation),
                    typeof (Collider),
                    typeof (Collider2D),
                    typeof (BoxCollider),
                    typeof (BoxCollider2D),
                    typeof (CapsuleCollider),
                    typeof (CharacterController),
                    typeof (MeshCollider),
                    typeof (Camera),
                    typeof (Animator),
                    typeof (LightProbeGroup),
                    typeof (AudioClip),
                    typeof (AudioSource),
                    typeof (List<int>),
                    typeof (Action<string>),
                    typeof (Debug),
                    typeof (UnityWebRequest),
                    typeof (WWWForm)
                };
                return list;
            }
        }

//        [LuaCallCSharp]
//        public static List<Type> LuaCallCSharp_UnityUI
//        {
//            get
//            {
//                var list = Assembly.Load("UnityEngine.UI").GetExportedTypes().Concat(CustomTypes)
//                    .Where(type => !type.IsGenericTypeDefinition)
//                    .Where(type => !type.IsNested)
//                    .Where(type => !IsExcluded(type)).ToList();
//                return list;
//            }
//        }

        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp_Unity
        {
            get
            {
                var list = Assembly.Load("UnityEngine").GetExportedTypes().Concat(CustomTypes)
                    .Where(type => !type.IsGenericTypeDefinition)
                    .Where(type => !type.IsNested)
                    .Where(type => !IsExcluded(type)).ToList();
                return list;
            }
        }

        private static bool IsExcluded(Type type)
        {
            var fullName = type.FullName;
            foreach (var value in Exclude)
            {
                if (fullName.Contains(value))
                {
                    return true;
                }
            }
            return false;
        }
    }
}