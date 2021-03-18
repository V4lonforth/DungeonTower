using DungeonTower.Level.Base;
using DungeonTower.Utils;
using System;

namespace DungeonTower.Controllers
{
    public class GameController : Singleton<GameController>
    {
        public GameState GameState { get; private set; } = GameState.Initializing;
        public Stage CurrentStage { get; private set; }

        public Action<Stage> OnStageStart { get; set; }
        public Action<Stage> OnStageFinish { get; set; }

        private void Start()
        {
            Stage stage = FindObjectOfType<StageGenerator>().Generate();
            StartStage(stage);
        }

        public void StartStage(Stage stage)
        {
            CurrentStage = stage;
            OnStageStart?.Invoke(stage);
        }
    }
}
