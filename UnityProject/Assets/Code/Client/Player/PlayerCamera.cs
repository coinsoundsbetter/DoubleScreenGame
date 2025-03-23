using UnityEngine;

namespace Code.Client.Player {
    public class PlayerCamera : MonoBehaviour {
        private Camera _cam;
        private Camera cam {
            get {
                if (_cam == null) {
                    _cam = GetComponent<Camera>();
                }

                return _cam;
            }
        }

        [SerializeField] private Vector3 defaultOffset = new Vector3(0, 0, 5f);
        [SerializeField] private float defaultFollowSpeed = 5f;
        [SerializeField] private float defaultRotateSpeed = 30f;
        
        private Transform followTarget;

        public void SetDisplay(int index) {
            cam.targetDisplay = index;
        }

        public void SetTarget(Transform target) {
            followTarget = target;
            transform.position = followTarget.position 
                                 - followTarget.forward * defaultOffset.z 
                                 - followTarget.right * defaultOffset.x
                                 - followTarget.up * defaultOffset.y;
        }

        public void UpdateMovement() {
            if (followTarget == null) {
                return;
            }
            
            var nowPos = transform.position;
            var endPos = followTarget.position 
                         - followTarget.forward * defaultOffset.z 
                         - followTarget.right * defaultOffset.x
                         - followTarget.up * defaultOffset.y;
            
            if (Vector3.SqrMagnitude(nowPos - endPos) > 0.01f) {
                nowPos = Vector3.MoveTowards(nowPos, endPos, Time.deltaTime * defaultFollowSpeed);
            }
            transform.position = nowPos;
            transform.LookAt(followTarget);
        }

        public Vector3 GetRight() => cam.transform.right;
        public Vector3 GetForward() => cam.transform.forward;
    }
}