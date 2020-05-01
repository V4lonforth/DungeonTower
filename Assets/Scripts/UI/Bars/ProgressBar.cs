using System;
using UnityEngine;

[Serializable]
public class ProgressBar 
{
    public int maxValue;

    public Action OnValueChanged;
    public Action OnEmptyBar;
    public Action OnFullBar;

    public bool Full => Value == maxValue;
    public bool Empty => Value == 0;

    public int Value
    {
        get => value;
        set
        {
            this.value = Mathf.Clamp(value, 0, maxValue);
            OnValueChanged?.Invoke();
            if (Full)
                OnFullBar?.Invoke();
            else if (Empty)
                OnEmptyBar?.Invoke();
        }
    }

    public float ValueF
    {
        get => (float)Value / maxValue;
        set => Value = Mathf.RoundToInt(value * maxValue);
    }

    private int value;

    public void Initialize()
    {
        Value = maxValue;
    }
} 