using UnityEngine;
using System.Collections;

namespace GameTemplate
{
    public class NetManager : MonoBehaviour
    {
        // Use this for initialization
        void Awake()
        {
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
