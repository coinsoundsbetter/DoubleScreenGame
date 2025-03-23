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
        [SerializeField] private Vector3 defaultFollowSpeed = new Vector3(30, 30, 5);
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
            nowPos.x = Mathf.MoveTowards(nowPos.x, endPos.x, Time.deltaTime * defaultFollowSpeed.x);
            nowPos.y = Mathf.MoveTowards(nowPos.y, endPos.y, Time.deltaTime * defaultFollowSpeed.y);
            nowPos.z = Mathf.MoveTowards(nowPos.z, endPos.z, Time.deltaTime * defaultFollowSpeed.z);
            transform.position = nowPos;
        }

        public Vector3 GetRight() => cam.transform.right;
        public Vector3 GetForward() => cam.transform.forward;
    }
}