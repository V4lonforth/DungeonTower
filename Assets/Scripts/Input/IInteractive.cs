using UnityEngine;

namespace DungeonTower.Input
{
    public interface IInteractive
    {
        int Layer { get; }
        bool Active { get; }
        bool HandleTouch(int touchId, Vector2 touchPosition, TouchPhase touchPhase);
    }
}
