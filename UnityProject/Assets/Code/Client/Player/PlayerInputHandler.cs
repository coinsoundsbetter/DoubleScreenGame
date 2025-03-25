using Code.Client.Input;
using UnityEngine;

namespace Code.Client.Player {
    public class PlayerInputHandler {
        private int playerIndex;
        
        // 移动
        public Vector2 SmoothMove { get; private set; }
        public Vector2 TargetMove { get; private set; }
        private Vector2 smoothMoveVelocity;
        private const float moveChangeDuration = 0.1f;
        
        // 旋转
        public Vector2 LookDelta { get; private set; }

        public void Init() {
            GameplayEvent.OnInputStateTick += OnHandleInputState;
        }
        
        public void Clear() {
            GameplayEvent.OnInputStateTick -= OnHandleInputState;
        }

        private void OnHandleInputState(int index, GameInput.PlayerInputState state) {
            SmoothMoveInput(state);
            LookDelta = state.LookDelta;
        }

        private void SmoothMoveInput(GameInput.PlayerInputState state) {
            TargetMove = state.MoveValue;
            SmoothMove = Vector2.SmoothDamp(SmoothMove, TargetMove, 
                ref smoothMoveVelocity, moveChangeDuration);
        }
    }
}