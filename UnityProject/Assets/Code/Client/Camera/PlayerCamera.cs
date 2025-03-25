using UnityEngine;

namespace Code.Client.Camera{
    public class PlayerCamera : MonoBehaviour {
        private UnityEngine.Camera _cam;
        private UnityEngine.Camera cam {
            get {
                if (_cam == null) {
                    _cam = GetComponent<UnityEngine.Camera>();
                }

                return _cam;
            }
        }

        [SerializeField] private Vector3 defaultOffset = new Vector3(0, 0, 5f);
        [SerializeField] private Vector3 defaultFollowSpeed = new Vector3(30, 30, 5);
        [SerializeField] private Vector2 defaultRotateSpeed = new Vector2(10, 10);
        [SerializeField] private Vector2 defaultXAngleLimit = new Vector2(-20F, 80F);
        
        private Transform followTarget;
        private Vector3 endPos;
        private Vector3 nowPos;
        private float rotateAroundY;
        private float rotateAroundX;

        public void SetTarget(Transform target) {
            followTarget = target;
            endPos = followTarget.position 
                     - followTarget.forward * defaultOffset.z 
                     - followTarget.right * defaultOffset.x
                     - followTarget.up * defaultOffset.y;
            transform.position = endPos;
            nowPos = endPos;
            rotateAroundX = 0;
            rotateAroundY = 0;
        }

        public void SetViewPort(Rect viewPort) {
            cam.rect = viewPort;
        }

        public void UpdateMovement(Vector2 roteDelta) {
            if (followTarget == null) {
                return;
            }
            
            // 累计视角旋转
            if (roteDelta != Vector2.zero) {
                rotateAroundX += -roteDelta.y * Time.deltaTime * defaultRotateSpeed.x;
                rotateAroundY += roteDelta.x * Time.deltaTime * defaultRotateSpeed.y;
                rotateAroundX = Mathf.Clamp(rotateAroundX, defaultXAngleLimit.x, defaultXAngleLimit.y);
                rotateAroundY = (rotateAroundY + 180) % 360 - 180;
            }
            
            // 计算旋转后的位置偏移
            Quaternion rote = Quaternion.Euler(rotateAroundX, rotateAroundY, 0);
            Vector3 offset = followTarget.forward * defaultOffset.z +
                             followTarget.right * defaultOffset.x +
                             followTarget.up * defaultOffset.y;
            endPos = followTarget.position - (rote * offset);
            
            // 平滑跟上
            nowPos.x = Mathf.MoveTowards(nowPos.x, endPos.x, Time.deltaTime * defaultFollowSpeed.x);
            nowPos.y = Mathf.MoveTowards(nowPos.y, endPos.y, Time.deltaTime * defaultFollowSpeed.y);
            nowPos.z = Mathf.MoveTowards(nowPos.z, endPos.z, Time.deltaTime * defaultFollowSpeed.z);
            transform.position = nowPos;
            
            // 注视角色
            Vector3 lookDir = followTarget.position - transform.position;
            if (lookDir.sqrMagnitude > 0.001f) {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = targetRotation;
            }
        }

        public Vector3 GetRight() => cam.transform.right;
        public Vector3 GetForward() => cam.transform.forward;
    }
}