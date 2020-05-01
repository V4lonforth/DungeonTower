using UnityEngine;

public interface IInteractive
{
    bool Press(Vector2 position, int id);
    bool Hold(Vector2 position, int id);
    bool Release(Vector2 position, int id);
}