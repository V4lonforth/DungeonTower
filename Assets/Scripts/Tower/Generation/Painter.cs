using UnityEngine;
using UnityEngine.Tilemaps;

public class Painter
{
    private TowerGenerator towerGenerator;

    public Painter(TowerGenerator towerGenerator)
    {
        this.towerGenerator = towerGenerator;
    }

    public void Paint()
    {
        PaintRoom(towerGenerator.Tower.Rooms[0]);
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

    public void PaintRoom(Room room)
    {
        PaintFloor(room);
        PaintWalls(room, true);

        foreach (Cell cell in room.Cells)
        {
            if (!cell.Destroyed)
            {
                if (cell.ConnectedCells[Direction.TopLeft] == null)
                    towerGenerator.wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.TopLeft.Shift.x, cell.Position.y + Direction.TopLeft.Shift.y, 2), room.Type.rightWallTile);
                else if (cell.ConnectedCells[Direction.TopLeft].Room != room)
                    PaintRightWall(cell.ConnectedCells[Direction.TopLeft], true);

                if (cell.ConnectedCells[Direction.TopRight] == null)
                    towerGenerator.wallsTilemap.SetTile(new Vector3Int(cell.Position.x + Direction.TopRight.Shift.x, cell.Position.y + Direction.TopRight.Shift.y, 1), room.Type.leftWallTile);
                else if (cell.ConnectedCells[Direction.TopRight].Room != room)
                    PaintLeftWall(cell.ConnectedCells[Direction.TopRight], true);
            }
        }
    }

    private void PaintFloor(Room room)
    {
        foreach (Cell cell in room.Cells)
            if (!cell.Destroyed)
                towerGenerator.floorTilemap.SetTile((Vector3Int)cell.Position, room.Type.floorTile);
    }

    private void PaintWalls(Room room, bool visible)
    {
        foreach (Cell cell in room.Cells)
            if (!cell.Destroyed)
                PaintWall(cell, visible);
    }

    private void PaintWall(Cell cell, bool visible)
    {
        PaintLeftWall(cell, visible);
        PaintRightWall(cell, visible);
    }

    private void PaintLeftWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.BottomLeft] != null)
        {
            if (cell.ConnectedCells[Direction.BottomLeft].Room != cell.Room)
                towerGenerator.wallsTilemap.SetTile(new Vector3Int(cell.Position.x, cell.Position.y, 1), visible ? cell.Room.Type.leftDoorTile : cell.Room.Type.leftDoorTransparentTile);
        }
        else
            towerGenerator.wallsTilemap.SetTile(new Vector3Int(cell.Position.x, cell.Position.y, 1), visible ? cell.Room.Type.leftWallTile : cell.Room.Type.leftWallTransparentTile);
    }
    private void PaintRightWall(Cell cell, bool visible)
    {
        if (cell.ConnectedCells[Direction.BottomRight] != null)
        {
            if (cell.ConnectedCells[Direction.BottomRight].Room != cell.Room)
                towerGenerator.wallsTilemap.SetTile(new Vector3Int(cell.Position.x, cell.Position.y, 2), visible ? cell.Room.Type.rightDoorTile : cell.Room.Type.rightDoorTransparentTile);
        }
        else
            towerGenerator.wallsTilemap.SetTile(new Vector3Int(cell.Position.x, cell.Position.y, 2), visible ? cell.Room.Type.rightWallTile : cell.Room.Type.rightWallTransparentTile);
    }
}