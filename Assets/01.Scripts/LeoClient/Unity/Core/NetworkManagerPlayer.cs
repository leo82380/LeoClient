using LeoClient.Unity.Network;
using LeoClient.Unity.Tool;
using UnityEngine;

namespace LeoClient.Unity.Core
{
    public class NetworkManagerPlayer : MonoBehaviour
    {
        [Header("Player Prefab")]
        [field: SerializeField] public LeoClientMono PlayerPrefab { get; private set; }

        private void Start()
        {
            NetworkManager.Instance.OnMessageReceived += HandleMessageReceived;
        }

        private void OnDestroy()
        {
            NetworkManager.Instance.OnMessageReceived -= HandleMessageReceived;
        }

        private void HandleMessageReceived(string message)
        {
            if (message.StartsWith("SpawnPlayer"))
            {
                // 예시: "SpawnPlayer:0,1,0;0,0,0,1" 형식의 메시지 파싱
                var parts = message.Split(':');
                if (parts.Length == 2)
                {
                    var paramParts = parts[1].Split(';');
                    if (paramParts.Length == 2)
                    {
                        Vector3 position = Parser.ToVector3(paramParts[0]);
                        Quaternion rotation = Parser.ToQuaternion(paramParts[1]);
                        SpawnPlayer(position, rotation);
                    }
                }
            }
        }

        public LeoClientMono SpawnPlayer(SpawnPacket packet)
        {
            return SpawnPlayer(packet.Position, packet.Rotation);
        }

        public LeoClientMono SpawnPlayer(Vector3? position = null, Quaternion? rotation = null)
        {
            if (PlayerPrefab == null)
            {
                Debug.LogError("PlayerPrefab이 할당되지 않았습니다.");
                return null;
            }
            var spawnPos = position ?? Vector3.zero;
            var spawnRot = rotation ?? Quaternion.identity;
            LeoClientMono player = Instantiate(PlayerPrefab, spawnPos, spawnRot);
            // 필요시 추가 초기화 코드 작성
            return player;
        }
    }

    public struct SpawnPacket
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public override string ToString()
        {
            return $"Position: {Position}, Rotation: {Rotation}";
        }
    }
}