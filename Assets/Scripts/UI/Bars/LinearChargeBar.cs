using UnityEngine;

public class LinearChargeBar : ChargeBar
{
    public Transform foregroundTransform;

    public override void Fill(float value)
    {
        foregroundTransform.localScale = new Vector3(value, 1f, 1f);
        foregroundTransform.localPosition = new Vector3((value - 1f) / (2f * MathHelper.PixelsInUnit), 0f, 0f);
    }
}