using System;
using System.Net.Sockets;
using LeoServer.Client;
using LeoServer.Core;
using UnityEngine;

namespace LeoClient.Unity.Network
{
    public class LeoClientNetwork
    {
        private ServerInfo serverInfo;
        private TcpClientConnection connection;

        public event Action<string> OnMessageReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public LeoClientNetwork(ServerInfo info)
        {
            serverInfo = info;
        }

        public void Connect()
        {
            try
            {
                TcpClient tcpClient = new TcpClient(serverInfo.Host, serverInfo.Port);
                connection = new TcpClientConnection(tcpClient);

                connection.StartReceiving(HandleMessageReceived, HandleDisconnected);

                connection.Connected();
                OnConnected?.Invoke();
                Debug.Log("[ServerClientMonoBehaviour] Connected to server!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ServerClientMonoBehaviour] Connection error: {e.Message}");
            }
        }

        private void HandleDisconnected()
        {
            OnDisconnected?.Invoke();
        }

        private void HandleMessageReceived(IClientConnection connection, string msg)
        {
            OnMessageReceived?.Invoke(msg); 
        }

        public void Disconnect()
        {
            if (connection != null && connection.IsConnected)
            {
                connection.Disconnect();
                connection = null;
                Debug.Log("[ServerClientMonoBehaviour] Disconnected from server.");
            }
        }

        public void Send(string message)
        {
            if (connection != null && connection.IsConnected)
            {
                connection.SendMessage(message);
            }
            else
            {
                Debug.LogWarning("[ServerClientMonoBehaviour] Cannot send message. Not connected.");
            }
        }
    }

    [Serializable]
    public class ServerInfo
    {
        public string Host = "127.0.0.1";
        public int Port = 7777;
    }
}