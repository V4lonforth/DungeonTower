using System;
using System.Collections.Generic;

[Serializable]
public class EnemyGroup
{
    public int weight = 1;

    public int minStrength;
    public int minSize;

    public List<WeightedCreature> enemies;

    public Creature GetRandomEnemy(int strength)
    {
        return MathHelper.GetRandomElement(enemies.FindAll(element => element.element.GetComponent<Enemy>().strength <= strength), enemy => enemy.weight)?.element;
    }
}