using System.Collections.Generic;
using UnityEngine;

public class Concealer : MonoBehaviour
{
    private struct FogOfWarShape
    {
        public Sprite sprite;
        public Direction direction;

        public FogOfWarShape(Sprite sprite, Direction direction)
        {
            this.sprite = sprite;
            this.direction = direction;
        }
    }

    public GameObject fogOfWarPrefab;

    public GameObject fogOfWarVertical;
    public GameObject fogOfWarHorizontal;
    public GameObject fogOfWarVerticalConnector;
    public GameObject fogOfWarHorizontalConnector;
    public GameObject fogOfWarInnerCorner;
    public GameObject fogOfWarOuterCorner;

    public Sprite[] fogOfWarSprites;

    private GameObject[,] fogOfWar;

    private Dictionary<int, FogOfWarShape> fogOfWarShapes;
    private Tower tower;

    private void Awake()
    {
        fogOfWarShapes = new Dictionary<int, FogOfWarShape>(16)
        {
            //{ 0b1111, new FogOfWarShape(fogOfWarSprites[5], Direction.Right) },

            { 0b0001, new FogOfWarShape(fogOfWarSprites[1], Direction.Top) },
            { 0b0010, new FogOfWarShape(fogOfWarSprites[1], Direction.Right) },
            { 0b0100, new FogOfWarShape(fogOfWarSprites[1], Direction.Left) },
            { 0b1000, new FogOfWarShape(fogOfWarSprites[1], Direction.Bottom) },

            { 0b1010, new FogOfWarShape(fogOfWarSprites[2], Direction.Right) },
            { 0b1100, new FogOfWarShape(fogOfWarSprites[2], Direction.Bottom) },
            { 0b0101, new FogOfWarShape(fogOfWarSprites[2], Direction.Left) },
            { 0b0011, new FogOfWarShape(fogOfWarSprites[2], Direction.Top) },

            { 0b0110, new FogOfWarShape(fogOfWarSprites[3], Direction.Right) },
            { 0b1001, new FogOfWarShape(fogOfWarSprites[3], Direction.Bottom) },

            { 0b0111, new FogOfWarShape(fogOfWarSprites[4], Direction.Top) },
            { 0b1011, new FogOfWarShape(fogOfWarSprites[4], Direction.Right) },
            { 0b1101, new FogOfWarShape(fogOfWarSprites[4], Direction.Left) },
            { 0b1110, new FogOfWarShape(fogOfWarSprites[4], Direction.Bottom) },

            { 0b0000, new FogOfWarShape(fogOfWarSprites[0], Direction.Right) }
        };
    }

    public void ConcealTower(Tower tower)
    {
        this.tower = tower;
        fogOfWar = new GameObject[tower.Size.y, tower.Size.x];

        ConcealOutside(tower);
        for (int i = 0; i < tower.Rooms.Length; i++)
            ConcealRoom(tower.Rooms[i]);
        RevealRoom(tower.Rooms[0]);
    }

    private void ConcealRoom(Room room)
    {
        GameObject roomFogOfWar = Instantiate(new GameObject("Fog of war"), room.transform);
        room.FogOfWar = roomFogOfWar;
        foreach (Cell cell in room.Cells)
        {
            fogOfWar[cell.Position.y, cell.Position.x] = Instantiate(fogOfWarPrefab, cell.transform);
            fogOfWar[cell.Position.y, cell.Position.x].transform.parent = roomFogOfWar.transform;
        }
    }

    private bool HasFogOfWar(Vector2Int position)
    {
        return !(MathHelper.InRange(position, tower.Size) && tower[position].Room.IsRevealed);
    }

    private void Flip(GameObject gameObject, bool flipX, bool flipY)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = flipX;
        spriteRenderer.flipY = flipY;
    }

    private void ShapeFogOfWar(Cell cell)
    {
        Destroy(fogOfWar[cell.Position.y, cell.Position.x]);
        GameObject fogOfWarParent = Instantiate(new GameObject(), cell.transform);
        fogOfWar[cell.Position.y, cell.Position.x] = fogOfWarParent;

        if (HasFogOfWar(cell.Position + Vector2Int.up))
        {
            Instantiate(fogOfWarHorizontal, fogOfWarParent.transform);
            if (HasFogOfWar(cell.Position + Vector2Int.right))
                Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform);
            else if (HasFogOfWar(cell.Position + Vector2Int.one))
                Instantiate(fogOfWarHorizontalConnector, fogOfWarParent.transform);
        }
        else
        {
            if (HasFogOfWar(cell.Position + Vector2Int.one))
            {
                if (HasFogOfWar(cell.Position + Vector2Int.right))
                    Instantiate(fogOfWarVerticalConnector, fogOfWarParent.transform);
                else
                    Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform);
            }
        }

        if (HasFogOfWar(cell.Position + Vector2Int.right))
        {
            Instantiate(fogOfWarVertical, fogOfWarParent.transform);
            if (HasFogOfWar(cell.Position + Vector2Int.down))
                Flip(Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform), false, true);
        }
        else
        {
            if (HasFogOfWar(cell.Position + new Vector2Int(1, -1)))
            {
                if (!HasFogOfWar(cell.Position + Vector2Int.down))
                    Flip(Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform), false, true);
                else
                    Flip(Instantiate(fogOfWarHorizontalConnector, fogOfWarParent.transform), false, true);
            }
        }

        if (HasFogOfWar(cell.Position + Vector2Int.down))
        {
            Flip(Instantiate(fogOfWarHorizontal, fogOfWarParent.transform), false, true);
            if (HasFogOfWar(cell.Position + Vector2Int.left))
                Flip(Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform), true, true);
        }
        else
        {
            if (HasFogOfWar(cell.Position + new Vector2Int(-1, -1)))
            {
                if (!HasFogOfWar(cell.Position + Vector2Int.left))
                    Flip(Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform), true, true);
            }
        }

        if (HasFogOfWar(cell.Position + Vector2Int.left))
        {
            Flip(Instantiate(fogOfWarVertical, fogOfWarParent.transform), true, false);
            if (HasFogOfWar(cell.Position + Vector2Int.up))
                Flip(Instantiate(fogOfWarInnerCorner, fogOfWarParent.transform), true, false);
            else if (HasFogOfWar(cell.Position + new Vector2Int(-1, 1)))
                Flip(Instantiate(fogOfWarVerticalConnector, fogOfWarParent.transform), true, false);
        }
        else
        {
            if (HasFogOfWar(cell.Position + new Vector2Int(-1, 1)))
            {
                if (!HasFogOfWar(cell.Position + Vector2Int.up))
                    Flip(Instantiate(fogOfWarOuterCorner, fogOfWarParent.transform), true, false);
            }
        }
    }

    private void RevealRoom(Room room)
    {
        room.IsRevealed = true;

        List<Room> revealingRooms = new List<Room>() { room };
        foreach (Room adjacentRoom in room.AdjacentRooms)
            if (adjacentRoom.IsRevealed)
            {
                revealingRooms.Add(adjacentRoom);
                foreach (Room secondAdjacentRoom in adjacentRoom.AdjacentRooms)
                    if (secondAdjacentRoom.IsRevealed && !revealingRooms.Contains(secondAdjacentRoom))
                        revealingRooms.Add(secondAdjacentRoom);
            }

        foreach (Room revealingRoom in revealingRooms)
            foreach (Cell cell in revealingRoom.Cells)
                ShapeFogOfWar(cell);
    }

    private void ConcealOutside(Tower tower)
    {
        for (Vector2Int position = new Vector2Int(-1, -1); position.x <= tower.Size.x; position.x++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
        for (Vector2Int position = new Vector2Int(-1, tower.Size.y); position.x <= tower.Size.x; position.x++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
        for (Vector2Int position = new Vector2Int(-1, 0); position.y <= tower.Size.y; position.y++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
        for (Vector2Int position = new Vector2Int(tower.Size.x, 0); position.y <= tower.Size.y; position.y++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
    }

    public void RevealConnectedRooms(Cell cell)
    {
        foreach (Cell connectedCell in cell.ConnectedCells)
            if (connectedCell && !ReferenceEquals(cell.Room, connectedCell.Room) && !connectedCell.Room.IsRevealed)
                RevealRoom(connectedCell.Room);
    }
}