using UnityEngine;

namespace Code.Client.Player {
    /// <summary>
    /// 本地玩家逻辑
    /// </summary>
    public class LocalPlayer : Player {
        private PlayerInput _input;
        private PlayerCamera _camera;
        private PlayerView _view;
        public int LocalPlayerIndex { get; set; }
        public override bool IsLocalPlayer => true;

        public override void Initialize() {
            // 创建角色实体
            var playerGo = Object.Instantiate(GameContext.Instance.GamePrefabs.PlayerView);
            _view = playerGo.GetComponent<PlayerView>();
            
            // 创建角色相机
            var cameraGo = Object.Instantiate(GameContext.Instance.GamePrefabs.PlayerCamera);
            _camera = cameraGo.GetComponent<PlayerCamera>();
            _camera.SetDisplay(LocalPlayerIndex);
            
            // 创建输入
            _input = new PlayerInput();
            _input.Bind(LocalPlayerIndex);
            
            // 设置相机跟随目标
            _camera.SetTarget(_view.GetCameraTarget());
        }

        public override void Update() {
            _input.Update();
            ControlBodyDir();
        }

        public override void LateUpdate() {
            _camera.UpdateMovement();
        }

        private void ControlBodyDir() {
            /*int xRotate = 0;
            if (_input.Move.x > 0) {
                xRotate = 1;
            }else if (_input.Move.x < 0) {
                xRotate = -1;
            }
            
            // 控制角色的左右转向
            Vector3 rightInPlane = default;
            if (xRotate == 1) {
                rightInPlane = new Vector3(_camera.GetRight().x, 0, _camera.GetRight().z);
            }else if (xRotate == -1) {
                rightInPlane = new Vector3(_camera.GetRight().x, 0, _camera.GetRight().z) * -1;
            }
            if (rightInPlane != Vector3.zero) {
                var targetRotation = Quaternion.LookRotation(rightInPlane, Vector3.up);
                _view.GetBodyDir().rotation = Quaternion.Slerp(_view.GetBodyDir().rotation, targetRotation, Time.deltaTime * 10f);
            }
            
            // 控制角色向镜头方向转向
            Vector3 forwardInPlane = default;*/
            
        }
    }
}