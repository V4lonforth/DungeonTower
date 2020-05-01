using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootGenerator
{
    private TowerGenerator towerGenerator;

    public LootGenerator(TowerGenerator towerGenerator)
    {
        this.towerGenerator = towerGenerator;
    }

    public void GenerateLoot()
    {
        foreach (Room room in towerGenerator.Tower.Rooms.Skip(1))
            room.Value = GenerateLoot(room, room.Strength);
    }

    private int GenerateLoot(Room room, int roomValue)
    {
        List<Cell> emptyCells = new List<Cell>(room.Cells);
        int currentValue = 0;
        while (currentValue < roomValue)
        {
            int index = Random.Range(0, emptyCells.Count);
            Item item = room.Type.lootGroup.GetRandomItem(roomValue);
            if (item == null)
                break;
            currentValue += item.value;
            Item.CreateInstance(item, emptyCells[index]);
            emptyCells.RemoveAt(index);
        }
        return currentValue;
    }
}