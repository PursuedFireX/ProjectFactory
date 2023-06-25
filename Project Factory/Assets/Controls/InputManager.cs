using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PFX
{
    public class InputManager : MonoBehaviour
    {
        #region Setup
        private InputController inputs;
        private static InputManager _instance;
        public static InputManager I
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            inputs = new InputController();
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void OnEnable()
        {
            inputs.Player.Enable();
        }

        private void OnDisable()
        {
            inputs.Player.Disable();
        }

        #endregion

        public Vector2 Move()
        {
            return inputs.Player.Move.ReadValue<Vector2>();
        }

        public float KeyRotate()
        {
            return inputs.Player.KeyboardRotation.ReadValue<float>();
        }

        public bool RightMouseClick(bool hold = false)
        {
            if (hold)
                return inputs.Player.RightMouseButton.IsPressed();
            else
                return inputs.Player.RightMouseButton.triggered;
        }

        public bool RightMouseRelease()
        {
            return inputs.Player.RightMouseButton.WasReleasedThisFrame();
        }

        public float Scroll()
        {
            return inputs.Player.MouseScroll.ReadValue<Vector2>().y;
        }

        public bool LeftMouseClick(bool hold = false)
        {
            if (hold)
                return inputs.Player.LeftMouseButton.IsPressed();
            else
                return inputs.Player.LeftMouseButton.triggered;
        }

        public bool LeftMouseRelease()
        {
            return inputs.Player.LeftMouseButton.WasReleasedThisFrame();
        }

        public bool MiddleMouseClick()
        {
            return inputs.Player.MiddleMouseButton.triggered;
        }

        public bool MiddleMouseRelease()
        {
            return inputs.Player.MiddleMouseButton.WasReleasedThisFrame();
        }

        public bool HoldShift()
        {
            return inputs.Player.Shift.IsPressed();
        }

        public bool ToggleChatConsole()
        {
            return inputs.Player.ToggleChatConsole.triggered;
        }

        public bool OpenChat()
        {
            return inputs.Player.TextChat.triggered;
        }

        public bool OpenDebugChat()
        {
            return inputs.Player.DebugChat.triggered;
        }

        public bool ToggleDebugOverlay()
        {
            return inputs.Player.ToggleDebugOverlay.triggered;
        }

        public bool Enter()
        {
            return inputs.Player.Enter.triggered;
        }
    }
}

