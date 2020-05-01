using UnityEngine;

public class MapInputController : IInteractive
{
    private InputController inputController;
    private Highlighter highlighter;
    private Inspector inspector;

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

    public MapInputController(InputController inputController, Highlighter highlighter, Inspector inspector)
    {
        this.inputController = inputController;
        this.highlighter = highlighter;
        this.inspector = inspector;
    }

    private Direction GetClosestDirection(Vector2 delta)
    {
        float angle = Mathf.Atan2(delta.y, delta.x);
        angle = (angle >= 0 ? angle : angle + Mathf.PI * 2f) * Mathf.Rad2Deg;
        float minAngle = 360f;
        Direction minDirection = Direction.Right;
        foreach (Direction direction in Direction.Values)
        {
            float diff = Mathf.Abs(direction.Angle - angle);
            diff = diff > 180 ? 360 - diff : diff;
            if (diff < minAngle)
            {
                minAngle = diff;
                minDirection = direction;
            }
        }
        return minDirection;
    }

    private void Swipe(Vector2 delta)
    {
        Vector2Int position = GetClosestDirection(delta).ShiftPosition(inputController.TowerGenerator.Tower.Player.Cell.Position);
        if (MathHelper.InRange(position, inputController.TowerGenerator.size))
            inputController.TowerGenerator.Tower.Player.TryMakeMove(new Target(inputController.TowerGenerator.Tower[position]));
    }

    private void HighlightDirection(Vector2 delta)
    {
        Vector2Int cellPosition = GetClosestDirection(delta).ShiftPosition(inputController.TowerGenerator.Tower.Player.Cell.Position);
        if (MathHelper.InRange(cellPosition, inputController.TowerGenerator.size))
            highlighter.Highlight(inputController.TowerGenerator.Tower[cellPosition]);
        else
            highlighter.ClearHighlight();
    }

    private void Inspect(Vector2Int towerPosition)
    {
        inspecting = true;
        Cell cell = inputController.TowerGenerator.Tower[towerPosition];
        if (cell.Entity is Creature creature)
        {
            inspector.ShowText(creature.GetDescription());
        }
        else 
        {
            if (cell.Items.Count > 0)
                inspector.ShowText(cell.Items[0].GetDescription());
            else
                inspector.ShowEmpty();
        }
    }

    private void StopInspecting()
    {
        closingInspector = false;
        inspector.HideText();
    }

    public bool Press(Vector2 position, int id)
    {
        if (pressed)
            return false;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, Camera.main.nearClipPlane)) + new Vector3(0.5f, 0.5f, 0f);
        Vector2Int towerPosition = inputController.TowerGenerator.Tower.WorldToTowerPoint(worldPosition);

        hitCell = MathHelper.InRange(towerPosition, inputController.TowerGenerator.size) && inputController.TowerGenerator.Tower[towerPosition].Room.IsRevealed;
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
            if (swiping)
            {
                HighlightDirection(position - lastTouchPosition);
            }
            else if (!closingInspector && !inspecting)
            {
                Vector2 delta = position - lastTouchPosition;
                lastTouchPosition = position;
                if (delta.sqrMagnitude > MinSwipeDistance * MinSwipeDistance)
                {
                    swiping = true;
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
                //if (inputController.TowerGenerator.Tower.Player.SelectedAbility != null)
                //    inputController.Tower.Interact(startTowerPosition);
            }
            else
                Swipe(position - lastTouchPosition);
        }
        highlighter.ClearHighlight();
        pressed = false;
        return true;
    }
}