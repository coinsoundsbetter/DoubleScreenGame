using System;
using System.Collections.Generic;
using Code.Features;
using Unity.Netcode;
using UnityEngine;

namespace Code {
    public class GameContext : MonoBehaviour {
        [SerializeField] private Camera mainCamera;
        public static GameContext Instance { get; private set; }
        public FeatureManager ClientFeatures { get; private set; }
        public FeatureManager ServerFeatures { get; private set; }
        public bool IsStartClient { get; private set; }
        public bool IsStartServer { get; private set; }

        private void Awake() {
            Instance = this;
        }
        
        private void OnDestroy() {
            ShutGame();
        }

        private void Update() {
            ClientFeatures?.Update();
            ServerFeatures?.Update();
        }
        
        private void OnGUI() {
            if (GUILayout.Button("以主机开始游戏")) {
                EnterGame(true, true);
            }
        }

        private void EnterGame(bool isServer, bool isClient) {
            IsStartServer = isServer;
            IsStartClient = isClient;
            
            // 创建服务器的基础功能
            if (isServer) {
                ServerFeatures = new FeatureManager();
                ServerFeatures.CreateBatch()
                    .Add<ServerConnectionManager>()
                    .Add<ServerPlayerManager>()
                    .OnCreateAll();
            }
            
            // 创建客户端的基础功能
            if (isClient) {
                ClientFeatures = new FeatureManager();
                ClientFeatures.CreateBatch()
                    .Add<ClientConnectionManager>()
                    .Add<ClientPlayerManager>()
                    .OnCreateAll();
            }
            
            // 执行网络连接
            var networkMgr = NetworkManager.Singleton;
            if (isServer && isClient) {
                networkMgr.StartHost();
            }else if (isServer) {
                networkMgr.StartServer();
            }else {
                networkMgr.StartClient();
            }
        }

        private void ShutGame() {
            ServerFeatures?.Destroy();
            ClientFeatures?.Destroy();
            var networkMgr = NetworkManager.Singleton;
            if (networkMgr) {
                networkMgr.Shutdown(true);
            }
            GameplayEvent.ClearAll();
        }
    }
}
