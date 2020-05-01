using System;
using UnityEngine;

public class Button : MonoBehaviour, IInteractive
{
    public Action<Vector2> onPress;
    public Action<Vector2> onHold;
    public Action<Vector2> onRelease;

    protected RectTransform area;

    private bool pressed;
    private int touchId;

    protected void Awake()
    {
        area = GetComponent<RectTransform>();
    }

    public virtual bool Press(Vector2 position, int id)
    {
        if (!pressed && Contains(position))
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

    public bool Contains(Vector2 position)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(area, position);
    }
}