using UnityEngine;

public abstract class ActiveAbility : MonoBehaviour
{
    public bool targetRequired;
    public int cooldown;

    public bool Selected => Creature.SelectedAbility == this;
    public bool Recharged => turnsToRecharge <= 0;
    public Creature Creature { get; private set; }

    private int turnsToRecharge;

    protected void Awake()
    {
        Creature = GetComponent<Creature>();
    }

    public void FinishMove()
    {
        if (turnsToRecharge > 0)
            turnsToRecharge--;
    }

    public void SelectAbility()
    {
        if (Creature.SelectedAbility != null)
            DeselectAbility();
        Creature.SelectedAbility = this;
    }
    public void DeselectAbility()
    {
        Creature.SelectedAbility = null;
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