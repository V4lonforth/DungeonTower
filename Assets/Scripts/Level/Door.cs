using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public Sprite closed;
    public Sprite opened1;
    public Sprite opened2;

    private bool isOpened;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Open(Direction direction)
    {
        if (!isOpened)
        {
            isOpened = true;
            if (direction == Direction.Right || direction == Direction.Top)
                spriteRenderer.sprite = opened1;
            else
                spriteRenderer.sprite = opened2;
        }
    }

    public void Close()
    {
        spriteRenderer.sprite = closed;
        isOpened = false;
    }
}