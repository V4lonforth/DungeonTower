using UnityEngine;
using UnityEngine.Tilemaps;

public class Highlighter : MonoBehaviour
{
    public Tilemap floorTilemap;
    public Tile highlightedTile;
    public int height;

    private Cell lastHighlightedCell;

    public void ClearHighlight()
    {
        if (lastHighlightedCell != null)
            floorTilemap.SetTile(new Vector3Int(lastHighlightedCell.Position.x, lastHighlightedCell.Position.y, height), null);
    }

    public void Highlight(Cell cell)
    {
        ClearHighlight();
        floorTilemap.SetTile(new Vector3Int(cell.Position.x, cell.Position.y, height), highlightedTile);
        lastHighlightedCell = cell;
    }
}