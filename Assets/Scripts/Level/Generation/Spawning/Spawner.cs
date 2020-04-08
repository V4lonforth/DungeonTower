using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Text goldText;

    public EnemyGroups enemyGroups;

    public GameObject playerPrefab;

    public void Spawn(Tower tower)
    {
        foreach (Room room in tower.Rooms.Skip(1))
            room.Strength = GenerateMonsters(new List<Cell>(room.Cells), CalculateRoomStrength(room));
        tower.Player = GeneratePlayer(tower[0, 0]);
    }

    private int GenerateMonsters(List<Cell> emptyCells, int roomStrength)
    {
        EnemyGroup enemyGroup = enemyGroups.GetRandomEnemyGroup(emptyCells.Count, roomStrength);
        int currentStrength = 0;
        while (emptyCells.Count > 0 && currentStrength < roomStrength)
        {
            int index = Random.Range(0, emptyCells.Count);
            GameObject enemy = enemyGroup.GetRandomEnemy(roomStrength - currentStrength);
            if (enemy == null)
                break;
            currentStrength += GenerateEnemy(enemy, emptyCells[index]);
            emptyCells.RemoveAt(index);
        }
        return 0;
    }

    private int CalculateRoomStrength(Room room)
    {
        return Random.Range(25, 75) * room.Cells.Count;
    }

    public int GenerateEnemy(GameObject enemy, Cell cell)
    {
        return ((EnemyEntity)CreatureEntity.Instantiate(enemy, cell)).strength;
    }

    public PlayerEntity GeneratePlayer(Cell cell)
    {
        return PlayerEntity.Instantiate(playerPrefab, cell, goldText);
    }
}