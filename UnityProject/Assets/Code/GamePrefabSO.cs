using UnityEngine;

namespace Code {
    [CreateAssetMenu(menuName = "SO/GamePrefabSO")]
    public class GamePrefabSO : ScriptableObject {
        public GameObject PlayerNet;
        public GameObject PlayerView;
        public GameObject PlayerCamera;
        public GameObject PlayerInputManager;
    }
}