using UnityEngine;

namespace Code.Client.Player {
    public class RemotePlayer : Player {
        private PlayerView _view;

        public override void Initialize() {
            // 创建角色实体
            var playerGo = Object.Instantiate(GameContext.Instance.GamePrefabs.PlayerView, 
                new Vector3(67.5f, 23.5f, 42.8f), Quaternion.identity);
            _view = playerGo.GetComponent<PlayerView>();
        }

        public override void Update() {
            SyncPosition();
        }

        private void SyncPosition() {
            var pos = LocalNet.Instance.GetPos(GameId);
            if (pos.HasValue) {
                _view.SetPosition(pos.Value);
            }
        }
    }
}