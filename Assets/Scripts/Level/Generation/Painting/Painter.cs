using UnityEngine;
using UnityEngine.Tilemaps;

public class Painter : MonoBehaviour
{
    public RoomStyle defaultRoom;

    public Tilemap floorTilemap;
    public Tilemap wallsTilemap;

    public Altar altar;
    public int altarSpawnChance;

    private Tower tower;

    public void Paint(Tower tower)
    {
        this.tower = tower;
        PaintRoom(tower.Rooms[0]);
    }

    public void PaintRoom(Room room)
    {
        PaintFloor(room);
        PaintWalls(room, true);

        if (Random.Range(0f, 1f) < altarSpawnChance)
            Altar.Instantiate(altar, room.Cells[Random.Range(0, room.Cells.Count)]);

        foreach (Cell cell in room.Cells)
        {
            if (!cell.Destroyed)
            {
                if (cell.ConnectedCells[Direction.TopLeft] == null)
                    wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.TopLeft.Shift.x, cell.Position.y + Direction.TopLeft.Shift.y, 2), defaultRoom.rightWallTile);
                else if (cell.ConnectedCells[Direction.TopLeft].Room != room)
                    PaintRightWall(cell.ConnectedCells[Direction.TopLeft], true);

                if (cell.ConnectedCells[Direction.TopRight] == null)
                    wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.TopRight.Shift.x, cell.Position.y + Direction.TopRight.Shift.y, 1), defaultRoom.leftWallTile);
                else if (cell.ConnectedCells[Direction.TopRight].Room != room)
                    PaintLeftWall(cell.ConnectedCells[Direction.TopRight], true);
            }
        }
    }

    public void PaintFloor(Room room)
    {
        foreach (Cell cell in room.Cells)
            if (!cell.Destroyed)
                floorTilemap.SetTile(cell.Position3, defaultRoom.floorTile);
    }

    public void PaintWalls(Room room, bool visible)
    {
        foreach (Cell cell in room.Cells)
            PaintWall(cell, visible);
    }

    public void SetVisibility(Room room, bool visible)
    {
        foreach (Cell cell in room.Cells)
        {
            if (cell.ConnectedCells[Direction.BottomLeft] == null || cell.ConnectedCells[Direction.BottomLeft].Room != room)
            {
                PaintWall(cell, visible);
                if (cell.AdjacentCells[Direction.BottomLeft] != null && (cell.AdjacentCells[Direction.BottomLeft].Room.IsRevealed || 
                    (cell.AdjacentCells[Direction.BottomLeft].AdjacentCells[Direction.BottomRight] != null &&
                    cell.AdjacentCells[Direction.BottomLeft].AdjacentCells[Direction.BottomRight].Room.IsRevealed)))
                    PaintRightWall(cell.AdjacentCells[Direction.BottomLeft], visible);
            }
            if (cell.ConnectedCells[Direction.BottomRight] == null || cell.ConnectedCells[Direction.BottomRight].Room != room)
            {
                PaintWall(cell, visible);
                if (cell.AdjacentCells[Direction.BottomRight] != null && (cell.AdjacentCells[Direction.BottomRight].Room.IsRevealed ||
                    (cell.AdjacentCells[Direction.BottomRight].AdjacentCells[Direction.BottomLeft] != null &&
                    cell.AdjacentCells[Direction.BottomRight].AdjacentCells[Direction.BottomLeft].Room.IsRevealed)))
                    PaintLeftWall(cell.AdjacentCells[Direction.BottomRight], visible);
            }
        }
    }

    private void PaintLeftWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.BottomLeft] != null)
        {
            if (cell.ConnectedCells[Direction.BottomLeft].Room != cell.Room)
                wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 1), visible ? defaultRoom.leftDoorTile : defaultRoom.leftDoorTransparentTile);
        }
        else
            wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 1), visible ? defaultRoom.leftWallTile : defaultRoom.leftWallTransparentTile);
    }
    private void PaintRightWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.BottomRight] != null)
        {
            if (cell.ConnectedCells[Direction.BottomRight].Room != cell.Room)
                wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 2), visible ? defaultRoom.rightDoorTile : defaultRoom.rightDoorTransparentTile);
        }
        else
            wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 2), visible ? defaultRoom.rightWallTile : defaultRoom.rightWallTransparentTile);
    }

    private void PaintWall(Cell cell, bool visible)
    {
        if (!cell.Destroyed)
        {
            PaintLeftWall(cell, visible);
            PaintRightWall(cell, visible);
        }
    }

    public void RemoveRow(int x)
    {
        for (int y = 0; y <= tower.Size.y; y++)
        {
            wallsTilemap.SetTile(new Vector3Int(x, y, 1), null);
            wallsTilemap.SetTile(new Vector3Int(x, y, 2), null);
        }
    }
}