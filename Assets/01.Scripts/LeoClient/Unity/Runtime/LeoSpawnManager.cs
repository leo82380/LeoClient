using LeoClient.Unity.Network;
using UnityEngine;

namespace LeoClient.Unity.Runtime
{
    public class LeoSpawnManager : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [field: SerializeField] public LeoClientMono PlayerPrefab { get; private set; }

        /// <summary>
        /// 서버 응답 시 호출: Player 프리팹(LeoClientMono 상속) 소환
        /// </summary>
        /// <param name="position">소환 위치 (기본값 Vector3.zero)</param>
        /// <param name="rotation">소환 회전 (기본값 Quaternion.identity)</param>
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
}