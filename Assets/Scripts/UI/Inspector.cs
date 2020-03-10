using UnityEngine;
using UnityEngine.UI;

public class Inspector : MonoBehaviour
{
    public Text text;

    private void Awake()
    {
        HideText();
    }

    public void ShowEmpty()
    {
        ShowText("Empty");
    }

    public void ShowText(string str)
    {
        text.text = str;
        gameObject.SetActive(true);
    }

    public void HideText()
    {
        gameObject.SetActive(false);
    }
}