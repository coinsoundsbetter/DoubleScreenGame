using Code.Client.Camera;
using UnityEngine;

namespace Code.Client.Player {
    /// <summary>
    /// 本地玩家逻辑
    /// </summary>
    public class LocalPlayer : Player {
        private PlayerInputHandler _input;
        private PlayerView _view;
        private PlayerCamera _camera;
        public int LocalPlayerIndex { get; set; }
        public override bool IsLocalPlayer => true;
        
        public override void Initialize() {
            // 创建角色实体
            var playerGo = Object.Instantiate(GameContext.Instance.GamePrefabs.PlayerView, 
                new Vector3(67.5f, 23.5f, 42.8f), Quaternion.identity);
            _view = playerGo.GetComponent<PlayerView>();
            
            // 创建角色相机
            var cameraGo = Object.Instantiate(GameContext.Instance.GamePrefabs.PlayerCamera);
            _camera = cameraGo.GetComponent<PlayerCamera>();
            
            // 创建输入
            _input = new PlayerInputHandler();
            _input.Init();
            
            // 设置相机默认跟随目标
            _camera.SetViewPort(new Rect(0, 0, 0.5f, 1f));
            _camera.SetTarget(_view.GetCameraTarget());
        }

        public override void Destroy() {
            _input.Clear();
        }

        public override void Update() {
            ControlBodyDir();
            ControlMove();
        }

        public override void LateUpdate() {
            _camera.UpdateMovement(_input.LookDelta);
        }

        private void ControlBodyDir() {
            var inputX = _input.TargetMove.x;
            var inputY = _input.TargetMove.y;
            if (inputX == 0 && inputY == 0) {
                return;
            }
            
            var cameraRight = _camera.GetRight().SetY(0);
            var cameraForward =_camera.GetForward().SetY(0);
            Vector3 inputDir = cameraRight * inputX + cameraForward * inputY;
            Quaternion bodyTargetRotation = Quaternion.LookRotation(inputDir);
            Quaternion bodyNowRotation = Quaternion.Slerp(_view.GetBodyRotation(), bodyTargetRotation, Time.deltaTime * 10f);
            _view.SetBodyRotation(bodyNowRotation);
        }

        private void ControlMove() {
            var inputX = _input.SmoothMove.x;
            var inputY = _input.SmoothMove.y;
            var inputNum = Mathf.Max(Mathf.Abs(inputX), Mathf.Abs(inputY));
            _view.SetMoveAnim(inputNum);
            
            Vector3 moveMotion = Vector3.zero;
            // 常规移动
            if (inputX != 0 || inputY != 0) {
                var moveDir = (_camera.GetRight() * inputX + _camera.GetForward() * inputY).normalized;
                var moveSpeed = 10f * inputNum;
                moveMotion = moveDir * moveSpeed * Time.deltaTime;
            }
            
            // 重力
            if (!_view.IsGrounded()) {
                moveMotion.y = -1 * Time.deltaTime;
            }
            
            // 应用移动
            _view.MoveOnce(moveMotion);
        }
    }
}