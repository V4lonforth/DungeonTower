public abstract class Furniture : Entity
{
    public static Furniture Instantiate(Furniture furniturePrefab, Cell cell)
    {
        Furniture furniture = Instantiate(furniturePrefab.prefab, cell).GetComponent<Furniture>();
        cell.Entity = furniture;
        //furniture.Tower.TowerGenerator.Linker.UnlinkCell(cell);
        return furniture;
    }
}