using DungeonTower.Controllers;
using DungeonTower.Entity.Base;
using DungeonTower.Level.Base;
using DungeonTower.Utils;
using UnityEngine;

namespace DungeonTower.Entity.CellBorder
{
    public class DoorEntity : MonoBehaviour, IPassable, IInteractableThrough
    {
        [SerializeField] private Sprite closed;
        [SerializeField] private Sprite opened1;
        [SerializeField] private Sprite opened2;

        private bool isOpened;

        public bool CanPass(CellEntity cellEntity)
        {
            return isOpened || cellEntity != null && cellEntity.Team == EntityTeam.Player;
        }

        public void Interact(CellEntity cellEntity, Direction direction)
        {
            OpenDoor(direction);
        }

        public void Pass(CellEntity cellEntity, Direction direction)
        {
            OpenDoor(direction);
        }

        private void OpenDoor(Direction direction)
        {
            if (!isOpened)
            {
                isOpened = true;

                if (direction == Direction.Right || direction == Direction.Top)
                    GetComponent<SpriteRenderer>().sprite = opened1;
                else
                    GetComponent<SpriteRenderer>().sprite = opened2;

                GetComponent<BorderEntity>().transparency = Transparency.Transparent;
            }
        }
    }
}
