using UnityEngine;

public class Concealer : MonoBehaviour
{
    public GameObject fogOfWarPrefab;

    private GameObject[,] fogOfWar;

    public void ConcealTower(Tower tower)
    {
        fogOfWar = new GameObject[tower.Size.y, tower.Size.x];

        ConcealOutside(tower);
        for (int i = 1; i < tower.Rooms.Length; i++)
            ConcealRoom(tower.Rooms[i]);
        tower.Rooms[0].IsRevealed = true;
    }

    private void ConcealRoom(Room room)
    {
        GameObject roomFogOfWar = Instantiate(new GameObject("Fog of war"), room.transform);
        room.FogOfWar = roomFogOfWar;
        foreach (Cell cell in room.Cells)
        {
            fogOfWar[cell.Position.y, cell.Position.x] = Instantiate(fogOfWarPrefab, cell.transform);
            fogOfWar[cell.Position.y, cell.Position.x].transform.parent = roomFogOfWar.transform;
        }
    }

    private void RevealRoom(Room room)
    {
        room.IsRevealed = true;
        room.FogOfWar.SetActive(false);
        //foreach (Cell cell in room.Cells)
        //    Destroy(fogOfWar[cell.Position.y, cell.Position.x]);
    }

    private void ConcealOutside(Tower tower)
    {
        for (Vector2Int position = new Vector2Int(-1, -1); position.x <= tower.Size.x; position.x++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
        for (Vector2Int position = new Vector2Int(-1, tower.Size.y); position.x <= tower.Size.x; position.x++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
        for (Vector2Int position = new Vector2Int(-1, 0); position.y <= tower.Size.y; position.y++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
        for (Vector2Int position = new Vector2Int(tower.Size.x, 0); position.y <= tower.Size.y; position.y++)
            Instantiate(fogOfWarPrefab, (Vector2)position, Quaternion.identity);
    }

    public void RevealConnectedRooms(Cell cell)
    {
        foreach (Cell connectedCell in cell.ConnectedCells)
            if (connectedCell && !ReferenceEquals(cell.Room, connectedCell.Room) && !connectedCell.Room.IsRevealed)
                RevealRoom(connectedCell.Room);
    }
}