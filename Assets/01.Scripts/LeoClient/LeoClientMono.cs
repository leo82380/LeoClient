using System;
using UnityEngine;

namespace LeoClient.Unity.Network
{
    public class LeoClientMono : MonoBehaviour
    {
        [Header("Server Info")]
        public ServerInfo serverInfo;

        private LeoClientHelper clientHelper = new LeoClientHelper();
        private LeoClientNetwork clientNetwork;

        // 이벤트
        public event Action<string> OnMessageReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public LeoClientNetwork Network => clientNetwork;

        protected virtual void Awake()
        {
            clientNetwork = new LeoClientNetwork(serverInfo);
            clientNetwork.OnMessageReceived += HandleMessageReceived;
            clientNetwork.OnConnected += HandleConnected;
            clientNetwork.OnDisconnected += HandleDisconnected;
        }

        protected virtual void Start()
        {
            clientNetwork.Connect();
        }

        protected virtual void Update()
        {
            clientHelper.ProcessQueue();
        }

        protected virtual void FixedUpdate()
        {
            // 필요한 경우 상속받은 클래스에서 사용
        }

        protected virtual void OnDestroy()
        {
            clientNetwork.OnMessageReceived -= HandleMessageReceived;
            clientNetwork.OnConnected -= HandleConnected;
            clientNetwork.OnDisconnected -= HandleDisconnected;
            clientNetwork.Disconnect();
        }
        
        private void HandleDisconnected()
        {
            clientHelper.EnqueueOnMainThread(() => OnDisconnected?.Invoke());
        }

        private void HandleConnected()
        {
            clientHelper.EnqueueOnMainThread(() => OnConnected?.Invoke());
        }

        private void HandleMessageReceived(string msg)
        {
            clientHelper.EnqueueOnMainThread(() => OnMessageReceived?.Invoke(msg));
        }
    }
}