using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code {
    public class GameContext : MonoBehaviour {
        public static GameContext Instance { get; private set; }
        public FeatureManager ClientFeatures { get; private set; }
        public FeatureManager ServerFeatures { get; private set; }
        public bool IsStartClient { get; private set; }
        public bool IsStartServer { get; private set; }

        public GamePrefabSO GamePrefabs;

        private void Awake() {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            IsStartServer = BoostrapConfig.Instance.IsStartServer;
            IsStartClient = BoostrapConfig.Instance.IsStartClient;
            SceneManager.LoadScene("rpgpp_lt_scene_1.0", LoadSceneMode.Additive);
            EnterGame();
        }

        private void OnDestroy() {
            ShutGame();
            Instance = null;
        }

        private void Update() {
            ClientFeatures?.Update();
            ServerFeatures?.Update();
        }

        private void LateUpdate() {
            ClientFeatures?.LateUpdate();
            ServerFeatures?.LateUpdate();
        }

        private void EnterGame() {
            // 创建服务器的基础功能
            if (IsStartServer) {
                ServerFeatures = new FeatureManager();
                ServerFeatures.CreateBatch()
                    .Add<Server.Connection.ConnectionManager>()
                    .Add<Server.Player.PlayerManager>()
                    .OnCreateAll();
            }
            
            // 创建客户端的基础功能
            if (IsStartClient) {
                ClientFeatures = new FeatureManager();
                ClientFeatures.CreateBatch()
                    .Add<Client.Input.GameInput>()
                    .Add<Client.Connection.ConnectionManager>()
                    .Add<Client.Player.PlayerManager>()
                    .OnCreateAll();
            }
            
            // 执行网络连接
            var networkMgr = NetworkManager.Singleton;
            switch (IsStartServer) {
                case true when IsStartClient:
                    networkMgr.StartHost();
                    break;
                case true:
                    networkMgr.StartServer();
                    break;
                default:
                    networkMgr.StartClient();
                    break;
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
