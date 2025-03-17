using Code.Mix;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Code.Server {
    public partial struct ServerGoInGameSystem : ISystem {
        
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GamePrefabs>();
            var requireQuery = SystemAPI.QueryBuilder().WithAll<RequestGoInGame, ReceiveRpcCommandRequest>().Build();
            state.RequireForUpdate(requireQuery);
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (request, requestEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>()
                         .WithAll<RequestGoInGame>()
                         .WithEntityAccess()) {
                ecb.AddComponent<NetworkStreamInGame>(request.ValueRO.SourceConnection);
                // Spawn Player
                var netId = SystemAPI.GetComponent<NetworkId>(request.ValueRO.SourceConnection);
                var playerPrefab = SystemAPI.GetSingleton<GamePrefabs>().Player;
                var player = ecb.Instantiate(playerPrefab);
                ecb.SetComponent(player, new GhostOwner() {
                    NetworkId = netId.Value,
                });
                ecb.AppendToBuffer(request.ValueRO.SourceConnection, new LinkedEntityGroup() {
                    Value = player,
                });
                ecb.DestroyEntity(requestEntity);
            }
            ecb.Playback(state.EntityManager);
        }
    }
}