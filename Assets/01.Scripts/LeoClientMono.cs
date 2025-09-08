using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using LeoServer.Core;
using LeoServer.Client;

public class LeoClientMono : MonoBehaviour
{
    [Header("Server Info")]
    public string Host = "127.0.0.1";
    public int Port = 7777;

    protected TcpClientConnection connection;
    private Queue<Action> mainThreadQueue = new Queue<Action>();

    // 이벤트
    public event Action<string> OnMessageReceived;
    public event Action OnConnected;
    public event Action OnDisconnected;

    #region Unity Methods

    // 유니티에서 상속받은 클래스에서도 Start/Update/FixedUpdate 사용 가능
    protected virtual void Start()
    {
        Connect();
    }

    protected virtual void Update()
    {
        ProcessQueue();
    }

    protected virtual void FixedUpdate()
    {
        // 필요한 경우 상속받은 클래스에서 사용
    }

    protected virtual void OnDestroy()
    {
        Disconnect();
    }

    #endregion

    #region Public API

    public void Connect()
    {
        try
        {
            TcpClient tcpClient = new TcpClient(Host, Port);
            connection = new TcpClientConnection(tcpClient);

            connection.StartReceiving((conn, msg) =>
            {
                EnqueueOnMainThread(() => OnMessageReceived?.Invoke(msg));
            }, () =>
            {
                EnqueueOnMainThread(() => OnDisconnected?.Invoke());
            });

            connection.Connected();
            EnqueueOnMainThread(() => OnConnected?.Invoke());
            Debug.Log("[ServerClientMonoBehaviour] Connected to server!");
        }
        catch (Exception e)
        {
            Debug.LogError($"[ServerClientMonoBehaviour] Connection error: {e.Message}");
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

    public void Disconnect()
    {
        if (connection != null && connection.IsConnected)
        {
            connection.Disconnect();
            connection = null;
            Debug.Log("[ServerClientMonoBehaviour] Disconnected from server.");
        }
    }

    #endregion

    #region Helper

    protected void EnqueueOnMainThread(Action action)
    {
        lock (mainThreadQueue)
        {
            mainThreadQueue.Enqueue(action);
        }
    }

    private void ProcessQueue()
    {
        while (true)
        {
            Action action = null;
            lock (mainThreadQueue)
            {
                if (mainThreadQueue.Count > 0)
                    action = mainThreadQueue.Dequeue();
            }

            if (action == null) break;
            action.Invoke();
        }
    }

    #endregion
}
