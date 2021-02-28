using UnityEngine;

public class AbilityController : MonoBehaviour, IInteractive
{
    public AbilityButton abilityButton;

    public void SetAbility(ActiveAbility activeAbility)
    {
        abilityButton.ActiveAbility = activeAbility;
    }

    public bool Press(Vector2 position, int id)
    {
        return abilityButton.Press(position, id);
    }

    public bool Hold(Vector2 position, int id)
    {
        return abilityButton.Hold(position, id);
    }

    public bool Release(Vector2 position, int id)
    {
        return abilityButton.Release(position, id);
    }
}