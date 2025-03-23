using UnityEngine;

namespace Code.Client.Player {
    public class PlayerView : MonoBehaviour {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController cc;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Transform bodyDir;
        
        public Transform GetCameraTarget() => cameraTarget;
        public Transform GetBodyDir() => bodyDir;
    }
}