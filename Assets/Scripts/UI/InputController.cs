using UnityEngine;

public class InputController : MonoBehaviour, IInteractive
{
    public Inventory Inventory { get; private set; }
    public MapInputController MapInputController { get; private set; }
    public AbilityController AbilityController { get; private set; }

    public Tower Tower { get; set; }
    public Player Player { get; set; }

    private void Awake()
    {
        Inventory = FindObjectOfType<Inventory>();
        AbilityController = FindObjectOfType<AbilityController>();
        MapInputController = new MapInputController(this, FindObjectOfType<Highlighter>());
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
        return Inventory.Press(position, id) || AbilityController.Press(position, id) || MapInputController.Press(position, id);
    }

    public bool Hold(Vector2 position, int id)
    {
        return Inventory.Hold(position, id) || AbilityController.Hold(position, id) || MapInputController.Hold(position, id);
    }

    public bool Release(Vector2 position, int id)
    {
        return Inventory.Release(position, id) || AbilityController.Release(position, id) || MapInputController.Release(position, id);
    }
}