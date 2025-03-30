using System.Collections.Generic;
using Code.Mix;

namespace Code.Client.Player {
    public class PlayerManager : IFeature, ILateUpdateFeature {
        private readonly Dictionary<int, PlayerNet> _allPlayerNets = new Dictionary<int, PlayerNet>();
        private readonly Dictionary<int, Player> _allPlayers = new Dictionary<int, Player>();
        public int LocalPlayerNum { get; private set; } = 0;

        public void OnCreate() {
            LocalNet.Create();
            GameplayEvent.OnPlayerNetSpawn += OnPlayerNetSpawn;
            GameplayEvent.OnPlayerNetDespawn += OnPlayerNetDespawn;
        }

        public void OnDestroy() {
            GameplayEvent.OnPlayerNetSpawn -= OnPlayerNetSpawn;
            GameplayEvent.OnPlayerNetDespawn -= OnPlayerNetDespawn;
        }
        
        public void OnUpdate(ref FeatureState state) {
            foreach (var player in _allPlayers.Values) {
                player.Update();
            }
        }

        public void OnLateUpdate(ref FeatureState state) {
            foreach (var player in _allPlayers.Values) {
                player.LateUpdate();
            }
        }

        private void OnPlayerNetSpawn(PlayerNet net) {
            int id = net.GameId.Value;
            Player player = null;
            if (net.IsOwner) {
                player = new LocalPlayer {
                    LocalPlayerIndex = LocalPlayerNum++,
                    GameId = id,
                };
            }else {
                player = new RemotePlayer() {
                    GameId = id,
                };
            }
            
            LocalNet.Instance.SetNetChannel(id, net);
            _allPlayers.Add(id, player);
            _allPlayerNets.Add(id, net);
            player.Initialize();
        }
        
        private void OnPlayerNetDespawn(PlayerNet net) {
            int id = net.GameId.Value;
            if (_allPlayers.TryGetValue(id, out var player)) {
                player.Destroy();
                _allPlayers.Remove(id);
            }

            _allPlayerNets.Remove(id);
        }
    }
}