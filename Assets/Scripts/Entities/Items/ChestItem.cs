using UnityEngine;

public class ChestItem : Item
{
    public Sprite openedSprite;

    public bool Opened { get; private set; }

    public override string GetDescription()
    {
        return "Chest";
    }

    public override void Use(Player player)
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

            foreach (Item item in Cell.Items)
                if (item != this)
                    item.GetComponent<SpriteRenderer>().enabled = false;

            player.SetTarget(player.Cell);
        }
    }
}