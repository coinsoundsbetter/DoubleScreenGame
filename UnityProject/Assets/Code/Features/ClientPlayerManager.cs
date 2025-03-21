using System.Collections.Generic;
using Code.Player;

namespace Code.Features {
    
    public class ClientPlayerManager : IFeature {
        public Dictionary<int, ClientPlayer> Players = new Dictionary<int, ClientPlayer>();
    }
}