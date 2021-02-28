﻿public class GoldItem : Item
{
    public int Amount { get; set; }

    public override void Use(Player player)
    {
        player.InputController.Inventory.Gold += Amount;
        Destroy();
    }

    public override string GetDescription()
    {
        return $"{Amount} gold";
    }
}