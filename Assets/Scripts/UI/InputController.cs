using UnityEngine;

public class InputController : MonoBehaviour
{
    public Inventory Inventory { get; private set; }

    public Tower Tower { get; set; }

    private void Awake()
    {
        Inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Inventory.Press(touch.position, touch.fingerId))
                        continue;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (Inventory.Hold(touch.position, touch.fingerId))
                        continue;
                    break;
                case TouchPhase.Ended:
                    if (Inventory.Release(touch.position, touch.fingerId))
                        continue;
                    Press(touch.position);
                    break;
            }
        }
        if (Input.touches.Length == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Inventory.Press(Input.mousePosition, 0))
                    return;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Inventory.Release(Input.mousePosition, 0))
                    return;
                Press(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                if (Inventory.Hold(Input.mousePosition, 0))
                    return;
            }
        }
    }

    private void Press(Vector2 position)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane)) + new Vector3(0.5f, 0.5f, 0f);
        Tower.Interact(WorldToTowerPoint(worldPosition));
    }

    private Vector2Int WorldToTowerPoint(Vector2 position)
    {
        position = position - (Vector2)Tower.transform.position;
        return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }
}