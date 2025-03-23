using System;
using Code.Mix;

namespace Code {
    public static class GameplayEvent {
        // 网络对象
        public static Action<PlayerNet> OnPlayerNetSpawn;
        public static Action<PlayerNet> OnPlayerNetDespawn;
        
        // 网络连接状态变更
        public static Action<int, ulong> OnPlayerConnected;
        public static Action<int, ulong> OnPlayerDisconnected;
        public static Action<int, ulong> OnPlayerReconnected;
        
        // 服务器专属
        public static Action<int, ulong> SpawnServerPlayer;

        public static void ClearAll() {
            OnPlayerNetSpawn = null;
            OnPlayerNetDespawn = null;
            OnPlayerConnected = null;
            OnPlayerDisconnected = null;
            OnPlayerReconnected = null;
        }
    }
}