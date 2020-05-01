using System;
using System.Collections.Generic;

[Serializable]
public class EnemyGroup
{
    [Serializable]
    public class WeightedEnemy
    {
        public float weight = 1f;
        public Enemy enemy;
    }

    public float weight = 1f;
    public int minStrength;
    public int minSize;

    public List<WeightedEnemy> enemies;

    public Enemy GetRandomEnemy(int strength)
    {
        return MathHelper.GetRandomElement(enemies.FindAll(element => element.enemy.strength <= strength), enemy => enemy.weight)?.enemy;
    }
}