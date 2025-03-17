using Code.Mix;
using Unity.Entities;
using UnityEngine;

public class GamePrefabsAuthoring : MonoBehaviour {
    public GameObject playerPrefab;

    private class Baker : Baker<GamePrefabsAuthoring> {
        
        public override void Bake(GamePrefabsAuthoring authoring) {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.None);
            AddComponent(entity, new GamePrefabs() {
                Player = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}
