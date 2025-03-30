using System.Collections.Generic;
using Code.Mix;
using UnityEngine;

namespace Code.Client.Player {
    public class LocalNet {
        private Dictionary<int, PlayerNet> nets = new();
        public static LocalNet Instance { get; private set; }
        
        public static void Create() {
            Instance = new LocalNet();
        }
        
        public void SetNetChannel(int gameId, PlayerNet net) {
            nets[gameId] = net;
        }

        private PlayerNet GetNet(int gameId) {
            return nets[gameId];
        }

        public void SendPos(int playerId, Vector3 position) {
            GetNet(playerId)?.SetPos(position);
        }

		public Vector3? GetPos(int playerId) {
            return GetNet(playerId)?.GetPos();
        }
    }
}