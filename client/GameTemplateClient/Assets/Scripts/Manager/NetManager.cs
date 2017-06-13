using UnityEngine;
using System.Collections;

namespace GameTemplate
{
    public class NetManager : MonoBehaviour
    {
        // Use this for initialization
        void Awake()
        {
            Messenger.AddListener(XEvent.FinishLoading, InitMain);
        }

        private void InitMain()
        {
            Debug.Log("开始初始化网络");

            NetCore.Init();
            NetSender.Init();
            NetReceiver.Init();

            NetCore.enabled = true;
        }

        private void FixedUpdate()
        {
            NetCore.Dispatch();
        }

        void Update()
        {
        }
    }
}
