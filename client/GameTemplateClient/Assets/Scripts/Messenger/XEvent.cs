using UnityEngine;
using System.Collections;

namespace GameTemplate
{
    public class XEvent
    {
        //UI界面进入关卡
        public const string StartGame = "StartGame";
        public const string ExitGame = "ExitGame";
        public const string FinishLoadDB = "FinishLoadDB";
        //markers
        public const string OnRecvMarkers = "OnRecvMarkers";
        //assetbundle
        public const string FinishLoadAssetBundle = "GE_FinishLoadAssetBundle";
        public const string SwitchResDependence = "GE_SwitchResDependence";
        //loading
        public const string FinishLoading = "FinishLoading";
    }


}