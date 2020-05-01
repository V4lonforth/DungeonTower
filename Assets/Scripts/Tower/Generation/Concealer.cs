using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Concealer
{
    private TowerGenerator towerGenerator;

    public Concealer(TowerGenerator towerGenerator)
    {
        this.towerGenerator = towerGenerator;
    }

    public void ConcealTower()
    {
        TileBase[] tileArray = Enumerable.Repeat(towerGenerator.fogOfWarTile, towerGenerator.size.x * towerGenerator.size.y).ToArray();
        BoundsInt bounds = new BoundsInt(new Vector3Int(0, 0, 0), new Vector3Int(towerGenerator.size.x, towerGenerator.size.y, 1));
        towerGenerator.fogOfWarTilemap.SetTilesBlock(bounds, tileArray);

        foreach (Cell cell in towerGenerator.Tower.Cells)
            Conceal(cell);

        RevealRoom(towerGenerator.Tower.Rooms[0]);
        towerGenerator.Painter.SetVisibility(towerGenerator.Tower.Rooms[0], false);
    }

    private void Conceal(Cell cell)
    {
        Conceal(cell.Entity);
        foreach (Item item in cell.Items)
            Conceal(item);
    }
    private void Reveal(Cell cell)
    {
        Reveal(cell.Entity);
        foreach (Item item in cell.Items)
            Reveal(item);
    }

    private void Conceal(Entity entity)
    {
        if (entity != null)
        {
            foreach (SpriteRenderer spriteRenderer in entity.GetComponentsInChildren<SpriteRenderer>())
                spriteRenderer.enabled = false;
            foreach (MeshRenderer meshRenderer in entity.GetComponentsInChildren<MeshRenderer>())
                meshRenderer.enabled = false;
        }
    }
    private void Reveal(Entity entity)
    {
        if (entity != null)
        {
            foreach (SpriteRenderer spriteRenderer in entity.GetComponentsInChildren<SpriteRenderer>())
                spriteRenderer.enabled = true;
            foreach (MeshRenderer meshRenderer in entity.GetComponentsInChildren<MeshRenderer>())
                meshRenderer.enabled = true;
        }
    }

    private void RevealRoom(Room room)
    {
        room.IsRevealed = true;

        foreach (Cell cell in room.Cells)
        {
            towerGenerator.fogOfWarTilemap.SetTile((Vector3Int)cell.Position, null);
            Reveal(cell);
        }

        towerGenerator.Painter.PaintRoom(room);
    }

    public void RevealConnectedRooms(Cell cell)
    {
        bool changed = false;
        foreach (Cell connectedCell in cell.ConnectedCells)
            if (connectedCell != null && cell.Room != connectedCell.Room && !connectedCell.Room.IsRevealed)
            {
                changed = true;
                RevealRoom(connectedCell.Room);
            }
        if (changed)
            towerGenerator.Painter.SetVisibility(cell.Room, false);
    }
}