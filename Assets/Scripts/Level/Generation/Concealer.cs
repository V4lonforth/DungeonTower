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

    public Sprite[] fogOfWarSprites;

    private GameObject[,] fogOfWar;

    private Dictionary<int, FogOfWarShape> fogOfWarShapes;

    private void Awake()
    {
        fogOfWarShapes = new Dictionary<int, FogOfWarShape>(16)
        {
            { 0b1111, new FogOfWarShape(fogOfWarSprites[5], Direction.Right) },

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

    private void ShapeFogOfWar(Cell cell)
    {
        int key = 0;
        foreach (Direction direction in Direction.Values)
        {
            key <<= 1;
            if (cell.AdjacentCells[direction] == null || !cell.AdjacentCells[direction].Room.IsRevealed)
                key |= 1;
        }
        if (fogOfWarShapes.TryGetValue(key, out FogOfWarShape shape))
        {
            fogOfWar[cell.Position.y, cell.Position.x].GetComponent<SpriteRenderer>().sprite = shape.sprite;
            fogOfWar[cell.Position.y, cell.Position.x].transform.rotation = Quaternion.Euler(0f, 0f, shape.direction.Rotation);
        }
    }

    private void RevealRoom(Room room)
    {
        room.IsRevealed = true;

        foreach (Cell cell in room.Cells)
        {
            bool revealed = true;
            foreach (Direction direction in Direction.Values)
                if (cell.AdjacentCells[direction] == null || !cell.AdjacentCells[direction].Room.IsRevealed)
                {
                    revealed = false;
                    ShapeFogOfWar(cell);
                    break;
                }
            if (revealed)
                fogOfWar[cell.Position.y, cell.Position.x].SetActive(false);
            foreach (Direction direction in Direction.Values)
                if (cell.AdjacentCells[direction] != null && cell.AdjacentCells[direction].Room != cell.Room && cell.AdjacentCells[direction].Room.IsRevealed)
                    ShapeFogOfWar(cell.AdjacentCells[direction]);
                    //fogOfWar[cell.AdjacentCells[direction].Position.y, cell.AdjacentCells[direction].Position.x].SetActive(false);
        }
        //foreach (Cell cell in room.Cells)
        //    if (cell.AdjacentRooms.Count > 0)
        //        foreach (Direction direction in Direction.Values)
        //            if (cell.AdjacentCells[direction] != null && !cell.AdjacentCells[direction].Room.IsRevealed)
        //                ShapeFogOfWar(cell.AdjacentCells[direction]);
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