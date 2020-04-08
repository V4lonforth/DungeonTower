using UnityEngine;

public class MapInputController : IInteractive
{
    private InputController inputController;

    private bool pressed;
    private int touchId;
    private Vector2 startTouchPosition;
    private Vector2 lastTouchPosition;
    private Vector2Int startTowerPosition;

    private bool hitCell;

    private bool swiping;

    private float holdingTime;
    private bool inspecting;
    private bool closingInspector;

    private const float MinSwipeDistance = 4f;
    private const float MinInspectTime = 0.25f;

    public MapInputController(InputController inputController)
    {
        this.inputController = inputController;
    }

    private void Swipe(Vector2 delta)
    {
        Vector2Int signs = new Vector2Int(delta.x >= 0 ? 1 : -1, delta.y >= 0 ? 1 : -1);
        delta *= signs;
        if (delta.x > delta.y)
            signs.y = 0;
        else
            signs.x = 0;
        inputController.Tower.Interact(inputController.Tower.Player.Cell.Position + signs);
    }

    private void Inspect(Vector2Int towerPosition)
    {
        inspecting = true;
        Cell cell = inputController.Tower[towerPosition];
        Creature creature = cell.Creature;
        if (creature == null)
        {
            if (cell.Items.Count > 0)
                inputController.Tower.TowerGenerator.inspector.ShowText(cell.Items[0].GetDescription());
            else
                inputController.Tower.TowerGenerator.inspector.ShowEmpty();
        }
        else
            inputController.Tower.TowerGenerator.inspector.ShowText(creature.GetDescription());
    }

    private void StopInspecting()
    {
        closingInspector = false;
        inputController.Tower.TowerGenerator.inspector.HideText();
    }

    public bool Press(Vector2 position, int id)
    {
        if (pressed)
            return false;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane)) + new Vector3(0.5f, 0.5f, 0f);
        Vector2Int towerPosition = WorldToTowerPoint(worldPosition);

        hitCell = MathHelper.InRange(towerPosition, inputController.Tower.Size) && inputController.Tower[towerPosition].Room.IsRevealed;
        pressed = true;
        touchId = id;
        startTouchPosition = position;
        lastTouchPosition = position;
        startTowerPosition = towerPosition;

        holdingTime = 0f;
        swiping = false;

        if (inspecting)
        {
            closingInspector = true;
            inspecting = false;
        }

        return true;
    }

    public bool Hold(Vector2 position, int id)
    {
        if (pressed && id == touchId)
        {
            if (!closingInspector && !swiping && !inspecting)
            {
                Vector2 delta = position - lastTouchPosition;
                lastTouchPosition = position;
                if (delta.sqrMagnitude > MinSwipeDistance * MinSwipeDistance)
                {
                    swiping = true;
                    Swipe(delta);
                }
                else if (hitCell)
                {
                    holdingTime += Time.deltaTime;
                    if (holdingTime >= MinInspectTime)
                    {
                        Inspect(startTowerPosition);
                    }
                }
            }
            return true;
        }
        return false;
    }

    public bool Release(Vector2 position, int id)
    {
        if (!inspecting)
        {
            if (closingInspector)
                StopInspecting();
            else if (!swiping)
            {
                if (inputController.Player.SelectedAbility == null)
                    inputController.Tower.Interact(inputController.Tower.Player.Cell.Position);
                else
                    inputController.Tower.Interact(startTowerPosition);
            }
        }
        pressed = false;
        return true;
    }

    private Vector2Int WorldToTowerPoint(Vector2 position)
    {
        position = position - (Vector2)inputController.Tower.transform.position;
        return new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }
}