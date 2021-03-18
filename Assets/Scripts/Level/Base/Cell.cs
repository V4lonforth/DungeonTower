using DungeonTower.Entity.Base;
using DungeonTower.Entity.CellBorder;
using DungeonTower.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonTower.Level.Base
{
    public class Cell
    {
        public ForegroundEntity FrontEntity { get; private set; }
        public List<BackgroundEntity> BackEntities { get; } = new List<BackgroundEntity>();
        public BorderEntity[] BorderEntities { get; } = new BorderEntity[Direction.DirectionsAmount];

        public Vector2Int StagePosition { get; private set; }
        public Vector2 WorldPosition { get; private set; }

        public Stage Stage { get; set; }
        public Room Room { get; set; }

        public Transform Transform { get; set; }

        public Action<Cell, ForegroundEntity> OnFrontAttach { get; set; }
        public Action<Cell, ForegroundEntity> OnFrontDetach { get; set; }
        public Action<Cell, BackgroundEntity> OnBackAttach { get; set; }
        public Action<Cell, BackgroundEntity> OnBackDetach { get; set; }

        public Cell(Vector2Int stagePosition)
        {
            StagePosition = stagePosition;
            WorldPosition = StagePosition;
        }

        public void AttachToFront(ForegroundEntity entity)
        {
            FrontEntity = entity;
            OnFrontAttach?.Invoke(this, entity);
        }
        public void DetachFront()
        {
            ForegroundEntity entity = FrontEntity;
            FrontEntity = null;
            OnFrontDetach?.Invoke(this, entity);
        }

        public void AttachToBack(BackgroundEntity entity)
        {
            BackEntities.Add(entity);
            OnBackAttach?.Invoke(this, entity);
        }
        public void DetachBack(BackgroundEntity entity)
        {
            if (BackEntities.Remove(entity))
            {
                OnBackDetach?.Invoke(this, entity);
            }
        }

        public void AttachToBorder(BorderEntity borderEntity, Direction direction)
        {
            BorderEntities[direction] = borderEntity;
        }
        public void DetachBorder(Direction direction)
        {
            BorderEntities[direction] = null;
        }
    }
}