using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[Serializable]
public class RoomType
{
    public int minSize;
    public int maxSize;
    public float squareness;

    public int minCellStrength;
    public int maxCellStrength;

    public List<EnemyGroup> enemyGroups;
    public LootGroup lootGroup;

    public TileBase floorTile;

    public TileBase leftWallTile;
    public TileBase rightWallTile;

    public TileBase leftWallTransparentTile;
    public TileBase rightWallTransparentTile;

    public TileBase leftDoorTile;
    public TileBase rightDoorTile;

    public TileBase leftDoorTransparentTile;
    public TileBase rightDoorTransparentTile;

    public EnemyGroup GetRandomEnemyGroup(int size, int strength)
    {
        return MathHelper.GetRandomElement(enemyGroups.FindAll(element => element.minSize <= size && element.minStrength <= strength), element => element.weight);
    }
}