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
        BoundsInt bounds = new BoundsInt(new Vector3Int(0, 0, 0), new Vector3Int(tower.Size.x, tower.Size.y, 1));
        fogOfWarTilemap.SetTilesBlock(bounds, tileArray);

        foreach (Cell cell in tower.Cells)
            Conceal(cell);

        RevealRoom(tower.Rooms[0]);
    }

    public void Conceal(Cell cell)
    {
        Conceal(cell.Creature);
        foreach (Item item in cell.Items)
            Conceal(item);
    }
    public void Reveal(Cell cell)
    {
        Reveal(cell.Creature);
        foreach (Item item in cell.Items)
            Reveal(item);
    }

    private void Conceal(Entity entity)
    {
        if (entity != null)
            foreach (SpriteRenderer spriteRenderer in entity.GetComponentsInChildren<SpriteRenderer>())
                spriteRenderer.enabled = false;
    }
    private void Reveal(Entity entity)
    {
        if (entity != null)
            foreach (SpriteRenderer spriteRenderer in entity.GetComponentsInChildren<SpriteRenderer>())
                spriteRenderer.enabled = true;
    }

    private void RevealRoom(Room room)
    {
        room.IsRevealed = true;

        foreach (Cell cell in room.Cells)
        {
            fogOfWarTilemap.SetTile((Vector3Int)cell.Position, null);
            Reveal(cell);
        }

        tower.TowerGenerator.Decorator.DecorateRoom(room);
    }

    public void RevealConnectedRooms(Cell cell)
    {
        bool changed = false;
        foreach (Cell connectedCell in cell.ConnectedCells)
            if (connectedCell != null && !ReferenceEquals(cell.Room, connectedCell.Room) && !connectedCell.Room.IsRevealed)
            {
                changed = true;
                RevealRoom(connectedCell.Room);
            }
        if (changed)
            tower.TowerGenerator.Decorator.SetVisibility(cell.Room, false);
    }
}