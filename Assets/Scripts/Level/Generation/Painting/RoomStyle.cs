using System;
using UnityEngine.Tilemaps;

[Serializable]
public class RoomStyle
{
    public TileBase floorTile;

    public TileBase leftWallTile;
    public TileBase rightWallTile;

    public TileBase leftWallTransparentTile;
    public TileBase rightWallTransparentTile;

    public TileBase leftDoorTile;
    public TileBase rightDoorTile;

    public TileBase leftDoorTransparentTile;
    public TileBase rightDoorTransparentTile;
}