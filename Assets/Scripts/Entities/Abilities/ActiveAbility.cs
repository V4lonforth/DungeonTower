﻿using UnityEngine;

public abstract class ActiveAbility : MonoBehaviour
{
    public bool targetRequired;
    public int cooldown;

    public bool Selected => CreatureEntity.SelectedAbility == this;
    public bool Recharged => turnsToRecharge <= 0;
    public CreatureEntity CreatureEntity { get; private set; }

    private int turnsToRecharge;

    protected void Awake()
    {
        CreatureEntity = GetComponent<CreatureEntity>();
    }

    public void FinishMove()
    {
        if (turnsToRecharge > 0)
            turnsToRecharge--;
    }

    public void SelectAbility()
    {
        if (CreatureEntity.SelectedAbility != null)
            DeselectAbility();
        CreatureEntity.SelectedAbility = this;
    }
    public void DeselectAbility()
    {
        CreatureEntity.SelectedAbility = null;
    }

    public bool Use(Cell cell)
    {
        if (Recharged && CanTarget(cell))
        {
            turnsToRecharge = cooldown;
            Activate(cell);
            DeselectAbility();
            return true;
        }
        return false;
    }

    public virtual bool CanTarget(Cell cell)
    {
        return !targetRequired || cell != null;
    }
    protected abstract void Activate(Cell cell);
}