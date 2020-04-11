using System;
using UnityEngine.Tilemaps;

[Serializable]
public class RoomDecorations
{
    public TileBase floorTile;
    public TileBase wallTile;
    public TileBase rightDoorTile;
    public TileBase leftDoorTile;
    public TileBase bothDoorsTile;

    public TileBase wallTransparentTile;

    public TileBase rightDoorTransparentTile;
    public TileBase leftDoorTransparentTile;
    public TileBase bothDoorsTransparentTile;
}