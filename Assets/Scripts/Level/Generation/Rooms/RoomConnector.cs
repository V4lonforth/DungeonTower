using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Level.Generation.Rooms
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/RoomConnector", order = 1)]
    public class RoomConnector : ScriptableObject
    {
        public RoomConnection[] ConnectRooms(Cell[,] cells, Room[] rooms)
        {
            List<RoomConnection> roomConnections = new List<RoomConnection>();
            List<Room> processedRooms = new List<Room>();
            foreach (Room room in rooms)
            {
                processedRooms.Add(room);
                roomConnections.AddRange(ConnectRoom(cells, processedRooms, room));
            }

            return roomConnections.ToArray();
        }

        private List<RoomConnection> ConnectRoom(Cell[,] cells, List<Room> processedRooms, Room room)
        {
            List<RoomConnection> potentialConnections = new List<RoomConnection>();
            Vector2Int stageSize = MathHelper.GetArraySize(cells);

            foreach (Cell cell in room.Cells)
            {
                Vector2Int topCellPosition = Direction.Top.ShiftPosition(cell.StagePosition);
                if (MathHelper.InRange(topCellPosition, stageSize) && !processedRooms.Contains(cells[topCellPosition.y, topCellPosition.x].Room))
                    potentialConnections.Add(new RoomConnection(cell, Direction.Top, room, cells[topCellPosition.y, topCellPosition.x].Room));

                Vector2Int rightCellPosition = Direction.Right.ShiftPosition(cell.StagePosition);
                if (MathHelper.InRange(rightCellPosition, stageSize) && !processedRooms.Contains(cells[rightCellPosition.y, rightCellPosition.x].Room))
                    potentialConnections.Add(new RoomConnection(cell, Direction.Right, room, cells[rightCellPosition.y, rightCellPosition.x].Room));
            }

            List<RoomConnection> roomConnections = new List<RoomConnection>();
            while (potentialConnections.Count != 0)
            {
                RoomConnection roomConnection = MathHelper.GetRandomElement(potentialConnections);
                roomConnections.Add(roomConnection);
                potentialConnections.RemoveAll(c => c.To == roomConnection.To);
            }

            return roomConnections;
        }
    }
}
