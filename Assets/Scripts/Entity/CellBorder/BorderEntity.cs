using DungeonTower.Level.Base;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Entity.CellBorder
{
    public class BorderEntity : MonoBehaviour
    {
        public Transparency transparency;

        protected Cell first;
        protected Cell second;
        protected Direction direction;

        public void Attach(Cell first, Cell second)
        {
            direction = Direction.GetDirection(first.StagePosition, second.StagePosition);

            this.first = first;
            first.AttachToBorder(this, direction);

            this.second = second;
            second.AttachToBorder(this, direction.Opposite);
        }

        public void Attach(Cell cell, Direction direction)
        {
            this.direction = direction;
            first = cell;
            first.AttachToBorder(this, direction);
        }

        public void Detach()
        {
            if (first != null)
            {
                first.DetachBorder(direction);
                first = null;
            }

            if (second != null)
            {
                second.DetachBorder(direction.Opposite);
                second = null;
            }
        }
    }
}
