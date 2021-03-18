using DungeonTower.Utils;
using System;
using UnityEngine;

namespace DungeonTower.Input
{
    public class TouchHandler : IInteractive
    {
        public Func<Vector2, bool> CheckHit { get; set; }
        public Action<Vector2> OnPress { get; set; }
        public Action<Vector2> OnHold { get; set; }
        public Action<Vector2> OnRelease { get; set; }

        public bool UsingWorldPosition { get; set; }

        public bool Pressed { get; private set; }
        public int TouchId { get; private set; }

        public bool Active { get; private set; }
        public int Layer { get; private set; }

        public TouchHandler(int layer = 0, Func<Vector2, bool> checkHit = null,
            Action<Vector2> onPress = null, Action<Vector2> onHold = null, Action<Vector2> onRelease = null, bool usingWorldPosition = true)
        {
            Layer = layer;
            CheckHit = checkHit;
            OnPress = onPress;
            OnHold = onHold;
            OnRelease = onRelease;
            UsingWorldPosition = usingWorldPosition;

            InputController.Instance.AddInteractive(this);
        }

        public bool HandleTouch(int touchId, Vector2 touchPosition, TouchPhase touchPhase)
        {
            Vector2 position = GetPosition(touchPosition);
            switch (touchPhase)
            {
                case TouchPhase.Began:
                    return Press(touchId, position);
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    return Hold(touchId, position);
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    return Release(touchId, position);
            }
            return false;
        }

        private bool Press(int touchId, Vector2 position)
        {
            if (!Pressed && (CheckHit == null || CheckHit(position)))
            {
                Pressed = true;
                TouchId = touchId;
                OnPress?.Invoke(position);
                return true;
            }
            return false;
        }

        private bool Hold(int touchId, Vector2 position)
        {
            if (Pressed && TouchId == touchId)
            {
                OnHold?.Invoke(position);
                return true;
            }
            return false;
        }

        private bool Release(int touchId, Vector2 position)
        {
            if (Pressed && TouchId == touchId)
            {
                OnRelease?.Invoke(position);
                Pressed = false;
                return true;
            }
            return false;
        }

        private Vector2 GetPosition(Vector2 touchPosition)
        {
            return UsingWorldPosition ? (Vector2)CameraController.Instance.Camera.ScreenToWorldPoint(touchPosition) : touchPosition;
        }

        public void Enable()
        {
            Active = true;
        }

        public void Disable()
        {
            Pressed = false;
            Active = false;
        }
    }
}
