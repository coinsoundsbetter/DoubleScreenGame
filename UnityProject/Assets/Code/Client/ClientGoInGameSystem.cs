using Code.Mix;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Code.Client {
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct ClientGoInGameSystem : ISystem {
        
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GamePrefabs>();
            state.RequireForUpdate<NetworkId>();
        }

        public void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (netId, entity) in 
                     SystemAPI.Query<RefRO<NetworkId>>()
                         .WithNone<NetworkStreamInGame>()
                         .WithEntityAccess()) {
                ecb.AddComponent<NetworkStreamInGame>(entity);
                
                // 向服务器发送进入游戏的请求
                var requestEnt = ecb.CreateEntity();
                ecb.AddComponent<RequestGoInGame>(requestEnt);
                ecb.AddComponent(requestEnt, new SendRpcCommandRequest() {
                    TargetConnection = entity,
                });
            }
            ecb.Playback(state.EntityManager);
        }
    }
}