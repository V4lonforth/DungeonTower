﻿using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int value;
    public Sprite icon;

    public ItemEntity ItemEntity { get; set; }

    public abstract void Use(PlayerEntity player);
    public abstract string GetDescription();
}