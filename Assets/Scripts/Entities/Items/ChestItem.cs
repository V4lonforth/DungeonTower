using UnityEngine;

public class ChestItem : Item
{
    public Sprite openedSprite;

    public bool Opened { get; private set; }

    public override string GetDescription()
    {
        return "Chest";
    }

    public override void Use(PlayerEntity player)
    {
        if (!Opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            Opened = true;

            int contentValue = (int)(value * Random.Range(0.5f, 1.5f));
            while (contentValue > 0)
            {
                contentValue -= player.Tower.TowerGenerator.LootGenerator.GenerateItem(Cell, false);
            }

            foreach (Item itemEntity in Cell.ItemEntities)
                if (itemEntity != this)
                    itemEntity.GetComponent<SpriteRenderer>().enabled = false;

            player.SetTarget(player.Cell);
        }
    }
}