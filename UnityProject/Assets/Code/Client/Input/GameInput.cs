using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Client.Input {
    public class GameInput : IFeature {
        private PlayerInputManager inputManager;
        private Dictionary<int, PlayerInput> inputs;
        private Dictionary<int, PlayerInputState> inputStates;
        public struct PlayerInputState {
            public Vector2 MoveValue;
            public Vector2 LookDelta;
        }
        
        public void OnCreate() {
            inputs = new Dictionary<int, PlayerInput>(2);
            inputStates = new Dictionary<int, PlayerInputState>(2);
            var instance = Object.Instantiate(GameContext.Instance.GamePrefabs.PlayerInputManager);
            inputManager = instance.GetComponent<PlayerInputManager>();
            inputManager.onPlayerJoined += OnPlayerJoined;
            inputManager.onPlayerLeft += OnPlayerLeft;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnDestroy() {
            inputManager.onPlayerJoined -= OnPlayerJoined;
            inputManager.onPlayerLeft -= OnPlayerLeft;
        }

#if UNITY_EDITOR
        private bool isEnableInput = true;
#endif
        
        public void OnUpdate(ref FeatureState state) {
#if UNITY_EDITOR
            if (UnityEngine.Input.GetKeyDown(KeyCode.T)) {
                isEnableInput = !isEnableInput;
            }
#endif
            if (!isEnableInput) {
                return;
            }
            
            foreach (var kvp in inputs) {
                var index = kvp.Key;
                var inputState = inputStates[index];
                UpdateInput(kvp.Value, ref inputState);
                inputStates[index] = inputState;
                GameplayEvent.OnInputStateTick?.Invoke(index, inputState);
            }
        }

        private void OnPlayerJoined(PlayerInput input) {
            inputs.Add(input.playerIndex, input);
            inputStates.Add(input.playerIndex, new PlayerInputState());
        }

        private void OnPlayerLeft(PlayerInput input) {
            inputs.Remove(input.playerIndex);
            inputStates.Remove(input.playerIndex);
        }
        
        private void UpdateInput(PlayerInput source, ref PlayerInputState state) {
            var actions = source.actions;
            var moveValue = actions["Move"].ReadValue<Vector2>();
            if (moveValue.x > 0) {
                moveValue.x = 1;
            }else if (moveValue.x < 0) {
                moveValue.x = -1;
            }
            if (moveValue.y > 0) {
                moveValue.y = 1;
            }else if(moveValue.y < 0) {
                moveValue.y = -1;
            }
            state.MoveValue = moveValue;

            var lookDelta = actions["Look"].ReadValue<Vector2>();
            state.LookDelta = lookDelta;
        }
    }
}