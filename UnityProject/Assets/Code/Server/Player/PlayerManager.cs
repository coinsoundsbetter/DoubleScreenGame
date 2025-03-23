using System.Collections.Generic;
using Code.Mix;
using Unity.Netcode;
using UnityEngine;

namespace Code.Server.Player {
    public class PlayerManager : IFeature {
        private readonly Dictionary<int, ServerPlayer> _players = new();
        private readonly Dictionary<int, PlayerNet> _playerNets = new();

        public void OnCreate() {
            GameplayEvent.OnPlayerNetSpawn += OnPlayerNetSpawn;
            GameplayEvent.OnPlayerNetDespawn += OnPlayerNetDespawn;
            GameplayEvent.SpawnServerPlayer += SpawnServerPlayer;
        }

        public void OnDestroy() {
            GameplayEvent.OnPlayerNetSpawn -= OnPlayerNetSpawn;
            GameplayEvent.OnPlayerNetDespawn -= OnPlayerNetDespawn;
            GameplayEvent.SpawnServerPlayer -= SpawnServerPlayer;
        }

        private void SpawnServerPlayer(int id, ulong connId) {
            var instance = Object.Instantiate(GameContext.Instance.GamePrefabs.PlayerNet);
            var networkObj = instance.GetComponent<NetworkObject>();
            var playerNet = networkObj.GetComponent<PlayerNet>();
            playerNet.GameId.Value = id;
            networkObj.SpawnAsPlayerObject(connId);
            var newServerPlayer = new ServerPlayer();
            newServerPlayer.Initialize();
            _players.Add(id, newServerPlayer);
        }

        private void OnPlayerNetSpawn(PlayerNet net) {
            _playerNets.Add(net.GameId.Value, net);
        }

        private void OnPlayerNetDespawn(PlayerNet net) {
            _playerNets.Remove(net.GameId.Value);
        }
    }
}