using UnityEngine;

public class InputController : MonoBehaviour
{
    public Tower Tower { get; set; }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended)
                Press(touch.position);
        }
        if (Input.touches.Length == 0 && Input.GetMouseButtonUp(0))
            Press(Input.mousePosition);
    }

    private void Press(Vector2 position)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane)) + new Vector3(0.5f, 0.5f, 0f);
        Tower.Interact(WorldToTowerPoint(worldPosition));
    }

    private Vector2Int WorldToTowerPoint(Vector2 position)
    {
        position = position - (Vector2)Tower.transform.position;
        return new Vector2Int((int)position.x, (int)position.y);
    }
}