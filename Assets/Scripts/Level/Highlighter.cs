using UnityEngine;
using UnityEngine.Tilemaps;

public class Highlighter : MonoBehaviour
{
    public Tilemap floorTilemap;
    public Tile highlightedTile;
    public int height;

    private Cell lashHighlightedCell;

    public void ClearHighlight()
    {
        if (lashHighlightedCell != null)
            floorTilemap.SetTile(new Vector3Int(lashHighlightedCell.Position.x, lashHighlightedCell.Position.y, height), null);
    }

    public void Highlight(Cell cell)
    {
        ClearHighlight();
        floorTilemap.SetTile(new Vector3Int(cell.Position.x, cell.Position.y, height), highlightedTile);
        lashHighlightedCell = cell;
    }
}