using UnityEngine;
using UnityEngine.Tilemaps;

public class Decorator : MonoBehaviour
{
    public RoomDecorations defaultRoom;

    public Tilemap floorTilemap;
    public Tilemap wallsTilemap;

    public void Decorate(Tower tower)
    {
        DecorateRoom(tower.Rooms[0]);
    }

    public void DecorateRoom(Room room)
    {
        DecorateFloor(room);
        DecorateWalls(room, true);

        foreach (Cell cell in room.Cells)
        {
            if (cell.ConnectedCells[Direction.Top] == null)
                wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.Top.Shift.x, cell.Position.y + Direction.Top.Shift.y, 2), defaultRoom.rightWallTile);
            else if (cell.ConnectedCells[Direction.Top].Room != room)
                DecorateRightWall(cell.ConnectedCells[Direction.Top], true);

            if (cell.ConnectedCells[Direction.Right] == null)
                wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.Right.Shift.x, cell.Position.y + Direction.Right.Shift.y, 1), defaultRoom.leftWallTile);
            else if (cell.ConnectedCells[Direction.Right].Room != room)
                DecorateLeftWall(cell.ConnectedCells[Direction.Right], true);
        }
    }

    public void DecorateFloor(Room room)
    {
        foreach (Cell cell in room.Cells)
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
            if (cell.ConnectedCells[Direction.Left] == null || cell.ConnectedCells[Direction.Left].Room != room)
            {
                DecorateWall(cell, visible);
                if (cell.AdjacentCells[Direction.Left] != null)
                    DecorateRightWall(cell.AdjacentCells[Direction.Left], visible);
            }
            if (cell.ConnectedCells[Direction.Bottom] == null || cell.ConnectedCells[Direction.Bottom].Room != room)
            {
                DecorateWall(cell, visible);
                if (cell.AdjacentCells[Direction.Bottom] != null)
                    DecorateLeftWall(cell.AdjacentCells[Direction.Bottom], visible);
            }
        }
    }

    private void DecorateLeftWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.Left] != null)
        {
            if (cell.ConnectedCells[Direction.Left].Room != cell.Room)
                wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 1), visible ? defaultRoom.leftDoorTile : defaultRoom.leftDoorTransparentTile);
        }
        else
            wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 1), visible ? defaultRoom.leftWallTile : defaultRoom.leftWallTransparentTile);
    }
    private void DecorateRightWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.Bottom] != null)
        {
            if (cell.ConnectedCells[Direction.Bottom].Room != cell.Room)
                wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 2), visible ? defaultRoom.rightDoorTile : defaultRoom.rightDoorTransparentTile);
        }
        else
            wallsTilemap.SetTile(cell.Position3 + new Vector3Int(0, 0, 2), visible ? defaultRoom.rightWallTile : defaultRoom.rightWallTransparentTile);
    }


    private void DecorateWall(Cell cell, bool visible)
    {
        DecorateLeftWall(cell, visible);
        DecorateRightWall(cell, visible);
    }

    private void CheckBorders(Tower tower)
    {
        for (int i = 0; i < tower.Size.y; i++)
            wallsTilemap.SetTile(new Vector3Int(tower.Size.x, i, 1), defaultRoom.leftWallTile);
        for (int i = 0; i < tower.Size.x; i++)
            wallsTilemap.SetTile(new Vector3Int(i, tower.Size.y, 2), defaultRoom.rightWallTile);
    }
}