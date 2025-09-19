using UnityEngine;

namespace LeoClient.Unity.Tool
{
    public static class Parser
    {
        // 예시: "SpawnPlayer:0,1,0;0,0,0,1" 형식의 메시지 파싱
        public static Vector3 ToVector3(string str)
        {
            var parts = str.Split(',');
            if (parts.Length != 3) return Vector3.zero;
            if (float.TryParse(parts[0], out float x) &&
                float.TryParse(parts[1], out float y) &&
                float.TryParse(parts[2], out float z))
            {
                return new Vector3(x, y, z);
            }
            return Vector3.zero;
        }

        public static Quaternion ToQuaternion(string str)
        {
            var parts = str.Split(',');
            if (parts.Length != 4) return Quaternion.identity;
            if (float.TryParse(parts[0], out float x) &&
                float.TryParse(parts[1], out float y) &&
                float.TryParse(parts[2], out float z) &&
                float.TryParse(parts[3], out float w))
            {
                return new Quaternion(x, y, z, w);
            }
            return Quaternion.identity;
        }
    }
}