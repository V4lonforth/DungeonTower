using UnityEngine;

public class Energy : MonoBehaviour
{
    public ProgressBar bar;
    public float restoreSpeed;

    private float RestoreTime => 1f / restoreSpeed;
    private float timeToRestore;

    private void Awake()
    {
        bar.Initialize();
    }

    private void Update()
    {
        if (!bar.Full)
        {
            timeToRestore -= Time.deltaTime;
            while (timeToRestore <= 0f)
            {
                timeToRestore += RestoreTime;
                bar.Value++;
            }
        }
    }

    public bool TryReduceEnergy(int value)
    {
        if (bar.Value >= value)
        {
            bar.Value -= value;
            return true;
        }
        return false;
    }
}