using DungeonTower.Controllers;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using UnityEngine;

namespace DungeonTower.Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private float minSwipeSpeed = 4f;
        [SerializeField] private float minHoldTime = 0.25f;

        private Cell hitCell;
        private Vector2 lastTouchPosition;
        private float holdingTime;

        private bool swiped;
        private bool isHolding;

        private TouchHandler touchHandler;
        private Stage stage;

        public Action<Direction> OnSwipe { get; set; }
        public Action<Cell> OnHold { get; set; }
        public Action<Cell> OnPress { get; set; }

        private void Awake()
        {
            GameController.Instance.OnStageStart += StartStage;
            touchHandler = new TouchHandler(layer: -1, onPress: Press, onHold: Hold, onRelease: Release);
        }

        private void StartStage(Stage s)
        {
            stage = s;
            touchHandler.Enable();
        }

        private void OnDestroy() => touchHandler.Disable();

        private void Press(Vector2 position)
        {
            lastTouchPosition = position;
            holdingTime = 0f;
            swiped = false;
            isHolding = false;
            hitCell = stage.GetCellSafe(GetStagePosition(position));
        }

        private void Hold(Vector2 position)
        {
            if (!swiped && !isHolding)
            {
                Vector2 delta = position - lastTouchPosition;
                lastTouchPosition = position;
                if (delta.sqrMagnitude > minSwipeSpeed * minSwipeSpeed)
                {
                    swiped = true;
                    Direction swipeDirection = GetSwipeDirection(delta);
                    OnSwipe?.Invoke(swipeDirection);
                    return;
                }

                holdingTime += Time.deltaTime;
                if (hitCell != null && holdingTime >= minHoldTime)
                {
                    isHolding = true;
                    OnHold?.Invoke(hitCell);
                }
            }
        }

        private void Release(Vector2 position)
        {
            if (!swiped && !isHolding)
            {
                OnPress?.Invoke(hitCell);
            }
        }

        private Direction GetSwipeDirection(Vector2 delta)
        {
            float angle = Mathf.Atan2(delta.y, delta.x);
            angle = (angle >= 0 ? angle : angle + Mathf.PI * 2f) * Mathf.Rad2Deg;
            float minAngle = 360f;

            Direction closestDirection = Direction.Right;
            foreach (Direction direction in Direction.Values)
            {
                float diff = Mathf.Abs(direction.RotationFloat - angle);
                if (diff < minAngle)
                {
                    minAngle = diff;
                    closestDirection = direction;
                }
            }

            return closestDirection;
        }

        private Vector2Int GetStagePosition(Vector2 position)
        {
            return stage.WorldToTowerPoint(position);
        }
    }
}
