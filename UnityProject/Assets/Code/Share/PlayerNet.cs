using Unity.Collections;
using Unity.Netcode;

namespace Code.Mix {
    public class PlayerNet : NetworkBehaviour {
        public NetworkVariable<int> GameId = new NetworkVariable<int>(0);
        public NetworkVariable<FixedString64Bytes> Name = new NetworkVariable<FixedString64Bytes>();

        public override void OnNetworkSpawn() {
            GameplayEvent.OnPlayerNetSpawn(this);
        }

        public override void OnNetworkDespawn() {
            GameplayEvent.OnPlayerNetDespawn(this);
        }
    }
}