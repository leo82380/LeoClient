using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestClient : LeoClientMono
{
    [SerializeField] private Text text;

    protected override void Start()
    {
        base.Start();
        OnMessageReceived += HandleText;
    }

    protected override void OnDestroy()
    {
        OnMessageReceived -= HandleText;
        base.OnDestroy();
    }

    private void HandleText(string obj)
    {
        text.text = obj;
    }

    protected override void Update()
    {
        base.Update();
        if (Keyboard.current.aKey.isPressed)
        {
            Send("a");
        }
    }
}
