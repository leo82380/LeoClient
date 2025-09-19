using System;
using LeoClient.Unity.Network;
using UnityEngine;

namespace LeoClient.Unity.Core
{
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        [Header("Server Info")]
        [field: SerializeField] public ServerInfo ServerInfo { get; private set; }

        private LeoClientNetwork clientNetwork;
        private LeoClientHelper clientHelper;

        // 이벤트
        public event Action<string> OnMessageReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;

        protected override void Awake()
        {
            base.Awake();
            ConnectToServer();
            AddEvents();
        }

        private void Start()
        {
            clientNetwork.Connect();
            clientNetwork.Send("SpawnPlayer:1,1,10;0,0,0,1");
        }

        private void Update()
        {
            clientHelper.ProcessQueue();
        }

        private void OnDestroy()
        {
            RemoveEvents();
            clientNetwork.Disconnect();
        }

        private void AddEvents()
        {
            clientNetwork.OnMessageReceived += HandleMessageReceived;
            clientNetwork.OnConnected += HandleConnected;
            clientNetwork.OnDisconnected += HandleDisconnected;
        }

        private void RemoveEvents()
        {
            clientNetwork.OnMessageReceived -= HandleMessageReceived;
            clientNetwork.OnConnected -= HandleConnected;
            clientNetwork.OnDisconnected -= HandleDisconnected;
        }

        private void ConnectToServer()
        {
            clientNetwork = new LeoClientNetwork(ServerInfo);
            clientHelper = new LeoClientHelper();
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