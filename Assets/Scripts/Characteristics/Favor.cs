using System;
using UnityEngine;
using UnityEngine.UI;

public class Favor : MonoBehaviour
{
    public Text favorText;

    public float timeToStartReducing;
    public float reducingSpeed;
    private float ReducingTime => 1f / reducingSpeed;

    public int Value
    {
        get => value;
        set
        {
            this.value = value;
            UpdateText();
        }
    }
    private int value;

    private bool reducing;
    private float timeToStartReducingLeft;
    private float timeToReduceLeft;

    private void Awake()
    {
        timeToStartReducingLeft = timeToStartReducing;
        Value = 0;
    }

    private void Update()
    {
        if (reducing)
        {
            timeToReduceLeft -= Time.deltaTime;
            while (timeToReduceLeft <= 0f)
            {
                ReduceFavor(1);
                timeToReduceLeft += ReducingTime;
            }
        }
        else
        {
            timeToStartReducingLeft -= Time.deltaTime;
            if (timeToStartReducingLeft <= 0)
            {
                reducing = true;
                timeToReduceLeft = ReducingTime;
            }
        }
    }

    public void AddFavor(Enemy enemy)
    {
        Value += enemy.strength;
        reducing = false;
        timeToStartReducingLeft = timeToStartReducing;
    }

    public void ReduceFavor(int amount)
    {
        Value = Value - amount < 0 ? 0 : Value - amount;
    }

    private void UpdateText()
    {
        favorText.text = Value.ToString();
    }
}