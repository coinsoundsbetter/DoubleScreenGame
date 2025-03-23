using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Client.Player {
    public class PlayerInput {
        private int bindIndex;
        private InputAction moveAction;
        private UnityEngine.InputSystem.PlayerInput useInput;
        public Vector2 Move { get; private set; }
        private Vector2 targetMove;
        private float moveChangeTimer;
        private const float moveChangeDuration = 0.2f;
        
        public void Bind(int localPlayerIndex) {
            bindIndex = localPlayerIndex;
            if (bindIndex == 0) {
                useInput = PlayerInputManager.instance.JoinPlayer(bindIndex, -1, "Keyboard&Mouse");
            }else if (bindIndex == 1) {
                useInput = PlayerInputManager.instance.JoinPlayer(bindIndex, -1, "Gamepad&Gamepad");
            }

            if (useInput == null) {
                return;
            }

            useInput.SwitchCurrentActionMap("Player");
            moveAction = useInput.currentActionMap.FindAction("Move");
            moveAction.performed += OnMove;
        }

        private void OnMove(InputAction.CallbackContext context) {
            moveChangeTimer = 0;
        }

        public void Update() {
            targetMove = moveAction.ReadValue<Vector2>();
            float moveT = moveChangeTimer / moveChangeDuration;
            Move = Vector2.Lerp(Move, targetMove, moveT * Time.deltaTime);
            if (moveT < 1) {
                moveChangeTimer += Time.deltaTime;
            }
            Debug.Log(Move);
        }

        public void Clear() {
            moveAction.Disable();
        }
    }
}