using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonTower.Level.Generation.Rooms
{
    [CreateAssetMenu(fileName = "Data", menuName = "TowerGeneration/RoomGenerator", order = 1)]
    public class RoomGenerator : ScriptableObject
    {
        [SerializeField] private int minSize = 1;
        [SerializeField] private int maxSize = 10;

        public Room[] Generate(Cell[,] cells)
        {
            Vector2Int towerSize = MathHelper.GetArraySize(cells);
            bool[,] usedCells = new bool[towerSize.y, towerSize.x];

            List<Room> rooms = new List<Room>();
            for (int y = 0; y < towerSize.y; y++)
            {
                for (int x = 0; x < towerSize.x; x++)
                {
                    if (!usedCells[y, x])
                    {
                        rooms.Add(GenerateRoom(cells[y, x], cells, usedCells));
                    }
                }
            }
            return rooms.ToArray();
        }

        private Room GenerateRoom(Cell fromCell, Cell[,] cells, bool[,] usedCells)
        {
            List<Cell> selectedCells = new List<Cell>();
            List<Cell> availableCells = new List<Cell>();
            int roomSize = Random.Range(minSize, maxSize + 1);

            AddSelectedCell(fromCell, selectedCells, availableCells, usedCells);

            while (selectedCells.Count < roomSize)
            {
                AddAvailableCells(selectedCells.Last(), availableCells, cells, usedCells);
                if (availableCells.Count == 0) { break; }

                AddSelectedCell(SelectNextCell(availableCells), selectedCells, availableCells, usedCells);
            }

            Room room = new Room(selectedCells.ToArray());
            foreach (Cell cell in room.Cells)
                cell.Room = room;
            return room;
        }

        private void AddSelectedCell(Cell cell, List<Cell> selectedCells, List<Cell> availableCells, bool[,] usedCells)
        {
            selectedCells.Add(cell);
            usedCells[cell.StagePosition.y, cell.StagePosition.x] = true;
            availableCells.Remove(cell);
        }

        private void AddAvailableCells(Cell fromCell, List<Cell> availableCells, Cell[,] cells, bool[,] usedCells)
        {
            foreach (Direction direction in Direction.Values)
            {
                Vector2Int shiftedPosition = direction.ShiftPosition(fromCell.StagePosition);
                if (MathHelper.InRange(shiftedPosition, cells))
                {
                    Cell cell = cells[shiftedPosition.y, shiftedPosition.x];
                    if (!usedCells[shiftedPosition.y, shiftedPosition.x] && !availableCells.Contains(cell))
                    {
                        availableCells.Add(cell);
                    }
                }
            }
        }

        private Cell SelectNextCell(List<Cell> availableCells)
        {
            return availableCells[Random.Range(0, availableCells.Count)];
        }
    }
}
