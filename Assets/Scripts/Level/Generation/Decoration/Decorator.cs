using UnityEngine;
using UnityEngine.Tilemaps;

public class Decorator : MonoBehaviour
{
    public RoomDecorations defaultRoom;

    public TileBase emptyWallTile;

    public Tilemap floorTilemap;
    public Tilemap wallsTilemap;

    public void Decorate(Tower tower)
    {
        foreach (Room room in tower.Rooms)
        {
            DecorateFloor(room);
            DecorateWalls(room, true);
            CheckBorders(room);
        }
        DecorateWalls(tower.Rooms[0], false);
    }

    public void DecorateFloor(Room room)
    {
        foreach (Cell cell in room.Cells)
            floorTilemap.SetTile(cell.Position, defaultRoom.floorTile);
    }

    public void DecorateWalls(Room room, bool visible)
    {
        Vector3Int[] positions = new Vector3Int[room.Cells.Count];
        TileBase[] tiles = new TileBase[room.Cells.Count];

        for (int i = 0; i < room.Cells.Count; i++)
        {
            positions[i] = room.Cells[i].Position;
            tiles[i] = DecorateWall(room.Cells[i], visible);
        }

        wallsTilemap.SetTiles(positions, tiles);
    }

    private TileBase DecorateWall(Cell cell, bool visible)
    {
        bool right = cell.ConnectedCells[Direction.Bottom] != null && cell.ConnectedCells[Direction.Bottom].Room != cell.Room;
        bool left = cell.ConnectedCells[Direction.Left] != null && cell.ConnectedCells[Direction.Left].Room != cell.Room;
        if (right)
        {
            if (left)
                return visible ? defaultRoom.bothDoorsTile : defaultRoom.bothDoorsTransparentTile;
            else
                return visible ? defaultRoom.rightDoorTile : defaultRoom.rightDoorTransparentTile;
        }
        else
        {
            if (left)
                return visible ? defaultRoom.leftDoorTile : defaultRoom.leftDoorTransparentTile;
            else
                return visible ? defaultRoom.wallTile : defaultRoom.wallTransparentTile;
        }
    }

    private void CheckBorders(Room room)
    {
        foreach (Cell cell in room.Cells)
            CheckBorders(cell);
    }

    private void CheckBorders(Cell cell)
    {
        if (cell.AdjacentCells[Direction.Top] == null)
        {
            Vector3Int offsetPosition = new Vector3Int(cell.Position.x + Direction.Top.Shift.x, cell.Position.y + Direction.Top.Shift.y, -1);
            wallsTilemap.SetTile(offsetPosition, defaultRoom.wallTile);
            offsetPosition += (Vector3Int)Direction.Left.Shift;
            if (wallsTilemap.GetTile(offsetPosition) == null)
                wallsTilemap.SetTile(offsetPosition, emptyWallTile);
        }
        if (cell.AdjacentCells[Direction.Right] == null)
        {
            Vector3Int offsetPosition = new Vector3Int(cell.Position.x + Direction.Right.Shift.x, cell.Position.y + Direction.Right.Shift.y, -1);
            wallsTilemap.SetTile(offsetPosition, defaultRoom.wallTile);
            offsetPosition += (Vector3Int)Direction.Bottom.Shift;
            if (wallsTilemap.GetTile(offsetPosition) == null)
                wallsTilemap.SetTile(offsetPosition, emptyWallTile);
        }
    }
}