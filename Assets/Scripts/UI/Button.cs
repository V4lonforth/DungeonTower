using UnityEngine;

public class Button : MonoBehaviour, IInteractive
{
    protected RectTransform area;

    private bool pressed;
    private int touchId;

    public virtual bool Press(Vector2 position, int id)
    {
        if (!pressed && CheckPosition(position))
        {
            pressed = true;
            touchId = id;
            return true;
        }
        return false;
    }

    public virtual bool Hold(Vector2 position, int id)
    {
        return pressed && touchId == id;
    }

    public virtual bool Release(Vector2 position, int id)
    {
        if (pressed && touchId == id)
        {
            pressed = false;
            return true;
        }
        return false;
    }

    public bool CheckPosition(Vector2 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(area, position);
    }
}