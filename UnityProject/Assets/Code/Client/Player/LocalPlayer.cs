using UnityEngine;

namespace Code.Client.Player {
    /// <summary>
    /// 本地玩家逻辑
    /// </summary>
    public class LocalPlayer : Player {
        private PlayerInputHandler _input;
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
            _input = new PlayerInputHandler();
            _input.Init();
            
            // 设置相机跟随目标
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
            _camera.UpdateMovement();
        }

        private void ControlBodyDir() {
            var inputX = _input.TargetMove.x;
            var inputY = _input.TargetMove.y;
            if (inputX == 0 && inputY == 0) {
                return;
            }
            
            var cameraRight = _camera.GetRight().SetY(0);
            var cameraForward = _camera.GetForward().SetY(0);
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
            if (inputX == 0 && inputY == 0) {
                return;
            }
            
            var moveDir = (_camera.GetRight() * inputX + _camera.GetForward() * inputY).normalized;
            var moveSpeed = 10f * inputNum;
            Vector3 moveMotion = moveDir * moveSpeed * Time.deltaTime;
            _view.MoveOnce(moveMotion);
        }
    }
}