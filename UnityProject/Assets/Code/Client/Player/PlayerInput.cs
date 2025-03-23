using UnityEngine;

namespace Code.Client.Player {
    public class PlayerInput {
        private InputSystem_Actions _actions = new();
        private int bindIndex;
        public Vector2 Move { get; private set; }
        
        public void Bind(int localPlayerIndex) {
            bindIndex = localPlayerIndex;
            if (bindIndex == 0) {
                _actions.PlayerA.Enable();
            }else if (bindIndex == 1) {
                _actions.PlayerB.Enable();
            }
        }

        public void Update() {
            if (bindIndex == 0) {
                var aActions = _actions.PlayerA;
                Move = aActions.Move.ReadValue<Vector2>();
            }else if (bindIndex == 1) {
                var bActions = _actions.PlayerB;
                Move = bActions.Move.ReadValue<Vector2>();
            }
            Debug.Log(Move);
        }
    }
}