using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomDecorations
{
    public List<WeightedGameObject> backgroundWalls;
    public List<WeightedGameObject> horizontalWalls;
    public List<WeightedGameObject> verticalWalls;
    public List<WeightedGameObject> connectorWalls;

    public static GameObject GetGameObject(List<WeightedGameObject> weightedGameObjects)
    {
        return MathHelper.GetRandomElement(weightedGameObjects, weightedGameObject => weightedGameObject.weight).element;
    }
}