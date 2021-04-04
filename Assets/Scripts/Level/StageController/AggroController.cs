using DungeonTower.Controllers;
using DungeonTower.Entity.MoveControllers;
using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Level.StageController
{
    public class AggroController : MonoBehaviour
    {
        private void Awake()
        {
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            stage.PlayerEntity.GetComponent<MoveController>().OnMoveMade += (m, a) => FindEnemiesToAggro(a.Target.Room);
        }

        private void FindEnemiesToAggro(Room room)
        {
            foreach (Cell cell in room.Cells)
            {
                if (cell.FrontEntity != null)
                {
                    MoveController moveController = cell.FrontEntity.GetComponent<MoveController>();
                    if (moveController != null && !moveController.Active)
                    {
                        moveController.Activate();
                    }
                }
            }
        }
    }
}
