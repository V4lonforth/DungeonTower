using UnityEngine;

public class ChargeBar : MonoBehaviour
{
    public Transform foregroundTransform;

    private void Awake()
    {
        Fill(0f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Fill(float value)
    {
        foregroundTransform.localScale = new Vector3(value, 1f, 1f);
        foregroundTransform.localPosition = new Vector3((value - 1f) / (2f * MathHelper.PixelsInUnit), 0f, 0f);
    }
}
