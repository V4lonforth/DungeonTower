using System;
using System.Collections.Generic;

[Serializable]
public class EnemyGroups
{
    public List<EnemyGroup> enemyGroups;

    public EnemyGroup GetRandomEnemyGroup(int size, int strength)
    {
        return MathHelper.GetRandomElement(enemyGroups.FindAll(element => element.minSize <= size && element.minStrength <= strength), element => element.weight);
    }
}