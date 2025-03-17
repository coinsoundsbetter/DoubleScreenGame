using Code.Mix;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour {

    private class Baker : Baker<PlayerAuthoring> {
        public override void Bake(PlayerAuthoring authoring) {
            var entity = GetEntity(authoring.gameObject, TransformUsageFlags.Dynamic);
            AddComponent<Player>(entity);
        }
    }
}
