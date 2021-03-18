using DungeonTower.Controllers;
using DungeonTower.Level.Base;
using UnityEngine;

namespace DungeonTower.Utils
{
    public class CameraController : Singleton<CameraController>
    {
        public Vector3 offset;
        public Transform followedObject;

        public Camera Camera { get; private set; }

        private void Awake()
        {
            Camera = Camera.main;
            GameController.Instance.OnStageStart += StartStage;
        }

        private void StartStage(Stage stage)
        {
            followedObject = stage.PlayerEntity.transform;
        }

        private void LateUpdate()
        {
            if (followedObject)
                transform.position = followedObject.position + offset;
        }
    }
}