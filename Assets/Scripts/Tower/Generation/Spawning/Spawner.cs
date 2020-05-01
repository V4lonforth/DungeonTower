using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner
{
    private TowerGenerator towerGenerator;

    public Spawner(TowerGenerator towerGenerator)
    {
        this.towerGenerator = towerGenerator;
    }

    public void Spawn()
    {
        foreach (Room room in towerGenerator.Tower.Rooms.Skip(1))
            room.Strength = GenerateMonsters(room, CalculateRoomStrength(room));
        towerGenerator.Tower.Player = (Player)Creature.CreateInstance(towerGenerator.playerPrefab, towerGenerator.Tower[0, 0]);
    }

    private int GenerateMonsters(Room room, int roomStrength)
    {
        List<Cell> emptyCells = room.Cells.FindAll(cell => cell.Entity == null);
        EnemyGroup enemyGroup = room.Type.GetRandomEnemyGroup(emptyCells.Count, roomStrength);
        if (enemyGroup == null)
            return 0;
        int currentStrength = 0;
        while (emptyCells.Count > 0 && currentStrength < roomStrength)
        {
            int index = Random.Range(0, emptyCells.Count);
            Enemy enemy = enemyGroup.GetRandomEnemy(roomStrength - currentStrength);
            if (enemy == null)
                break;
            currentStrength += enemy.strength;
            Creature.CreateInstance(enemy, emptyCells[index]);
            emptyCells.RemoveAt(index);
        }
        return currentStrength;
    }

    private int CalculateRoomStrength(Room room)
    {
        return Random.Range(room.Type.minCellStrength, room.Type.maxCellStrength + 1) * room.Cells.Count;
    }
}