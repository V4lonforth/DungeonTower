using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Creature
{
    public int energyToMove;
    public int energyToAttack;

    public Inventory Inventory { get; private set; }

    private Energy energy;

    protected new void Awake()
    {
        base.Awake();
        energy = GetComponent<Energy>();
        Camera.main.GetComponent<CameraHelper>().followedObject = transform;
        Inventory = FindObjectOfType<Inventory>();
    }

    public bool TryMakeMove(Target target)
    {
        if (CanMove(target))
        {
            if (energy.TryReduceEnergy(energyToMove))
            {
                MakeMove(MoveTo, target);
                return true;
            }
        }
        else if (weapon.CanAttack(target))
        {
            if (energy.TryReduceEnergy(energyToAttack))
            {
                MakeMove(weapon.Attack, target);
                return true;
            }
        }
        return false;
    }

    private void MakeMove(Action<Target> move, Target target)
    {
        StartMakingMove(target.Cell);
        move?.Invoke(target);
        FinishMakingMove();
    }

    protected void StartMakingMove(Cell cell)
    {
        FaceCell(cell);
        cell.Room.AggroEnemies();
    }

    protected void FinishMakingMove()
    {
        Tower.TowerGenerator.Concealer.RevealConnectedRooms(Cell);
        Tower.Navigator.CreateMap(Cell);
        Inventory.itemDrop.Show(Cell.Items);
    }

    public override string GetDescription()
    {
        return "You";
    }

    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}