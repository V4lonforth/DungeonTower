using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Concealer : MonoBehaviour
{
    public Tilemap fogOfWarTilemap;
    public TileBase fogOfWarTile;

    private Tower tower;

    public void ConcealTower(Tower tower)
    {
        this.tower = tower;

        TileBase[] tileArray = Enumerable.Repeat(fogOfWarTile, tower.Size.x * tower.Size.y).ToArray();
        BoundsInt bounds = new BoundsInt(new Vector3Int(1, 1, 1), new Vector3Int(tower.Size.x, tower.Size.y, 1));
        fogOfWarTilemap.SetTilesBlock(bounds, tileArray);

        RevealRoom(tower.Rooms[0]);
    }
    
    private void RevealRoom(Room room)
    {
        room.IsRevealed = true;

        foreach (Cell cell in room.Cells)
            fogOfWarTilemap.SetTile((Vector3Int)cell.Position2 + Vector3Int.one, null);
    }

    public void RevealConnectedRooms(Cell cell)
    {
        foreach (Cell connectedCell in cell.ConnectedCells)
            if (connectedCell != null && !ReferenceEquals(cell.Room, connectedCell.Room) && !connectedCell.Room.IsRevealed)
                RevealRoom(connectedCell.Room);
    }
}