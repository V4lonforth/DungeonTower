using UnityEngine;

public abstract class ChargeBar : MonoBehaviour
{
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public abstract void Fill(float value);
}