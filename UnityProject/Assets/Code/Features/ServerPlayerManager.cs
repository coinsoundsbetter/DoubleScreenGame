using System.Collections.Generic;
using Code.Player;
using Unity.Netcode;
using UnityEngine;

namespace Code.Features {
    public class ServerPlayerManager : IFeature {
        private readonly Dictionary<int, ServerPlayer> players = new();

        public void OnCreate() {
            GameplayEvent.OnPlayerNetSpawn += OnPlayerNetSpawn;
            GameplayEvent.OnPlayerNetDespawn += OnPlayerNetDespawn;
            GameplayEvent.SpawnServerPlayer += SpawnServerPlayer;
        }

        private void SpawnServerPlayer(int id, ulong connId) {
            var asset = Resources.Load<PlayerNet>("PlayerNet");
            var instance = Object.Instantiate(asset);
            var networkObj = instance.GetComponent<NetworkObject>();
            var playerNet = networkObj.GetComponent<PlayerNet>();
            playerNet.GameId.Value = id;
            networkObj.SpawnAsPlayerObject(connId);
            var newServerPlayer = new ServerPlayer();
            players.Add(id, newServerPlayer);
        }

        private void OnPlayerNetSpawn(PlayerNet net) {
            if (players.TryGetValue(net.GameId.Value, out ServerPlayer player)) {
                player.SyncNet = net;
            }
        }
        
        private void OnPlayerNetDespawn(PlayerNet net) {
            if (players.TryGetValue(net.GameId.Value, out ServerPlayer player)) {
                player.SyncNet = null;
            }
        }
    }
}