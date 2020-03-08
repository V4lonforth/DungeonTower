using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public WeaponItem defaultWeapon;
    public ArmorItem defaultArmour;

    public PlayerEntity PlayerEntity { get; private set; }

    public WeaponItem EquippedWeapon { get; private set; }
    public ArmorItem EquippedArmor { get; private set; }

    public Item[] Backpack { get; private set; }

    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            UpdateText();
        }
    }
    private int gold;
    private TextMeshPro goldText;

    private const int BackpackSize = 3;

    private void Awake()
    {
        PlayerEntity = GetComponent<PlayerEntity>();

        Backpack = new Item[BackpackSize];

        defaultWeapon?.Use(PlayerEntity);
        defaultArmour?.Use(PlayerEntity);
    }

    public void SetText(TextMeshPro goldText)
    {
        this.goldText = goldText;
        UpdateText();
    }

    private void UpdateText()
    {
        if (goldText != null)
            goldText.text = Gold.ToString();
    }

    public void Equip(Item item)
    {
        item.ItemEntity?.Detach();
        item.transform.SetParent(PlayerEntity.transform);
        item.gameObject.SetActive(false);
    }
}