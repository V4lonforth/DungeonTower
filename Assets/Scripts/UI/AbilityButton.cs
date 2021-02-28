using UnityEngine;

public class AbilityButton : Button
{
    public ActiveAbility ActiveAbility { get; set; }

    private void Awake()
    {
        area = GetComponent<RectTransform>();
    }

    public override bool Release(Vector2 position, int id)
    {
        if (base.Release(position, id) && CheckPosition(position))
        {
            if (ActiveAbility.Selected)
                ActiveAbility.DeselectAbility();
            else
                ActiveAbility.SelectAbility();
            return true;
        }
        return false;
    }
}