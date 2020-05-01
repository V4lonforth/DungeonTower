using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour, IInteractive
{
    public List<Button> Buttons { get; private set; }
    public MapInputController MapInputController { get; private set; }
    public TowerGenerator TowerGenerator { get; private set; }

    private void Awake()
    {
        Buttons = new List<Button>();
        MapInputController = new MapInputController(this, GetComponent<Highlighter>(), GetComponent<Inspector>());
        TowerGenerator = FindObjectOfType<TowerGenerator>();
    }

    private void Update()
    {
        HandleTouches();
        HandleMouse();
    }

    private void HandleTouches()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Press(touch.position, touch.fingerId);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Hold(touch.position, touch.fingerId);
                    break;
                case TouchPhase.Ended:
                    Release(touch.position, touch.fingerId);
                    break;
            }
        }
    }

    private void HandleMouse()
    {
        if (Input.touches.Length == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Press(Input.mousePosition, 0);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Release(Input.mousePosition, 0);
            }
            else if (Input.GetMouseButton(0))
            {
                Hold(Input.mousePosition, 0);
            }
        }
    }

    public bool Press(Vector2 position, int id)
    {
        foreach (Button button in Buttons)
            if (button.Press(position, id))
                return true;
        return MapInputController.Press(position, id);
    }

    public bool Hold(Vector2 position, int id)
    {
        foreach (Button button in Buttons)
            if (button.Hold(position, id))
                return true;
        return MapInputController.Hold(position, id);
    }

    public bool Release(Vector2 position, int id)
    {
        foreach (Button button in Buttons)
            if (button.Release(position, id))
                return true;
        return MapInputController.Release(position, id);
    }
}