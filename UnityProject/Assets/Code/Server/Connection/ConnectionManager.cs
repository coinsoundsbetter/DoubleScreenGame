using System.Collections.Generic;
using Unity.Netcode;

namespace Code.Server.Connection {
    /// <summary>
    /// 维护局内的所有客户端连接状态
	/// 同时负责分配给每个客户端一个唯一的局内Id
    /// </summary>
    public class ConnectionManager : IFeature {
        private NetworkManager net;
        private Dictionary<ulong, int> connToGameId = new Dictionary<ulong, int>();
        private HashSet<int> connecting = new HashSet<int>();
        private HashSet<int> disconnected = new HashSet<int>();
        public int UniquePlayerNum { get; private set; }
        
        public void OnCreate() {
            net = NetworkManager.Singleton;
            net.OnClientConnectedCallback += OnClientConnected;
            net.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }

        public void OnDestroy() {
            net.OnClientConnectedCallback -= OnClientConnected;
            net.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }

        private void OnClientConnected(ulong connId) {
            // 如果是第一次进入游戏,分配一个局内Id
            // 如果是重连进入,广播重连信息,并刷新记录的连接状态
            if (!connToGameId.TryGetValue(connId, out int gameId)) {
                var newGameId = ++UniquePlayerNum;
                connToGameId.Add(connId, newGameId);
                connecting.Add(newGameId);
                GameplayEvent.OnPlayerConnected?.Invoke(newGameId, connId);
                GameplayEvent.SpawnServerPlayer?.Invoke(newGameId, connId);
            }else {
                disconnected.Remove(gameId);
                connecting.Add(gameId);
                GameplayEvent.OnPlayerReconnected?.Invoke(gameId, connId);
            }
        }
        
        private void OnClientDisconnectCallback(ulong connId) {
            if (!connToGameId.TryGetValue(connId, out var gameId)) {
                return;
            }

            connecting.Remove(gameId);
            disconnected.Add(gameId);
            GameplayEvent.OnPlayerDisconnected?.Invoke(gameId, connId);
        }
    }
}