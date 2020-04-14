using UnityEngine;
using UnityEngine.Tilemaps;

public class Decorator : MonoBehaviour
{
    public RoomDecorations defaultRoom;

    public Tilemap floorTilemap;
    public Tilemap wallsTilemap;

    private Tower tower;

    public void Decorate(Tower tower)
    {
        this.tower = tower;
        DecorateRoom(tower.Rooms[0]);
    }

    public void DecorateRoom(Room room)
    {
        DecorateFloor(room);
        DecorateWalls(room, true);

        foreach (Cell cell in room.Cells)
        {
            if (!cell.Destroyed)
            {
                if (cell.ConnectedCells[Direction.TopLeft] == null)
                    wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.TopLeft.Shift.x, cell.Position.y + Direction.TopLeft.Shift.y, 2), defaultRoom.rightWallTile);
                else if (cell.ConnectedCells[Direction.TopLeft].Room != room)
                    DecorateRightWall(cell.ConnectedCells[Direction.TopLeft], true);

                if (cell.ConnectedCells[Direction.TopRight] == null)
                    wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.TopRight.Shift.x, cell.Position.y + Direction.TopRight.Shift.y, 1), defaultRoom.leftWallTile);
                else if (cell.ConnectedCells[Direction.TopRight].Room != room)
                    DecorateLeftWall(cell.ConnectedCells[Direction.TopRight], true);
            }
        }
    }

    public void DecorateFloor(Room room)
    {
        foreach (Cell cell in room.Cells)
            if (!cell.Destroyed)
                floorTilemap.SetTile(cell.Position3, defaultRoom.floorTile);
    }

    public void DecorateWalls(Room room, bool visible)
    {
        foreach (Cell cell in room.Cells)
            DecorateWall(cell, visible);
    }

    public void SetVisibility(Room room, bool visible)
    {
        foreach (Cell cell in room.Cells)
        {
            if (cell.ConnectedCells[Direction.BottomLeft] == null || cell.ConnectedCells[Direction.BottomLeft].Room != room)
            {
                DecorateWall(cell, visible);
                if (cell.AdjacentCells[Direction.BottomLeft] != null && (cell.AdjacentCells[Direction.BottomLeft].Room.IsRevealed || 
                    (cell.AdjacentCells[Direction.BottomLeft].AdjacentCells[Direction.BottomRight] != null &&
                    cell.AdjacentCells[Direction.BottomLeft].AdjacentCells[Direction.BottomRight].Room.IsRevealed)))
                    DecorateRightWall(cell.AdjacentCells[Direction.BottomLeft], visible);
            }
            if (cell.ConnectedCells[Direction.BottomRight] == null || cell.ConnectedCells[Direction.BottomRight].Room != room)
            {
                DecorateWall(cell, visible);
                if (cell.AdjacentCells[Direction.BottomRight] != null && (cell.AdjacentCells[Direction.BottomRight].Room.IsRevealed ||
                    (cell.AdjacentCells[Direction.BottomRight].AdjacentCells[Direction.BottomLeft] != null &&
                    cell.AdjacentCells[Direction.BottomRight].AdjacentCells[Direction.BottomLeft].Room.IsRevealed)))
                    DecorateLeftWall(cell.AdjacentCells[Direction.BottomRight], visible);
            }
        }
    }

    private void DecorateLeftWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.BottomLeft] != null)
        {
            if (cell.ConnectedCells[Direction.BottomLeft].Room != cell.Room)
                wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 1), visible ? defaultRoom.leftDoorTile : defaultRoom.leftDoorTransparentTile);
        }
        else
            wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 1), visible ? defaultRoom.leftWallTile : defaultRoom.leftWallTransparentTile);
    }
    private void DecorateRightWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.BottomRight] != null)
        {
            if (cell.ConnectedCells[Direction.BottomRight].Room != cell.Room)
                wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 2), visible ? defaultRoom.rightDoorTile : defaultRoom.rightDoorTransparentTile);
        }
        else
            wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 2), visible ? defaultRoom.rightWallTile : defaultRoom.rightWallTransparentTile);
    }

    private void DecorateWall(Cell cell, bool visible)
    {
        if (!cell.Destroyed)
        {
            DecorateLeftWall(cell, visible);
            DecorateRightWall(cell, visible);
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