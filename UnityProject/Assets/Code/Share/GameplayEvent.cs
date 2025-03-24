using System;
using Code.Client.Input;
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
        
        // 输入状态
        public static Action<int, GameInput.PlayerInputState> OnInputStateTick;

        public static void ClearAll() {
            OnPlayerNetSpawn = null;
            OnPlayerNetDespawn = null;
            OnPlayerConnected = null;
            OnPlayerDisconnected = null;
            OnPlayerReconnected = null;
            SpawnServerPlayer = null;
            OnInputStateTick = null;
        }
    }
}