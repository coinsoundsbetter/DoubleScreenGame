using UnityEngine;

namespace Code.Client.Player {
    public class PlayerView : MonoBehaviour {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController cc;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Transform bodyDir;
        
        public Transform GetCameraTarget() => cameraTarget;
        public Quaternion GetBodyRotation() => bodyDir.rotation;

        public void SetBodyRotation(Quaternion rotation) {
            bodyDir.rotation = rotation;
        }

        public void MoveOnce(Vector3 motion) {
            cc.Move(motion);
        }

        public void SetMoveAnim(float moveValue) {
            animator.SetFloat("Move", moveValue);
        }

        public void SetPosition(Vector3 position) {
            transform.position = position;
        }

        public bool IsGrounded() {
            return cc.isGrounded;
        }
    }
}