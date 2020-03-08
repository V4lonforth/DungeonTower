using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemEntity ItemEntity { get; set; }

    public abstract void Use(PlayerEntity player);
}