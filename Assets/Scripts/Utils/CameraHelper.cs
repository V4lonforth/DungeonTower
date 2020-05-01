using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    public int cellAmount;
    public Vector3 offset;
    public Transform followedObject;

    private void Awake()
    {
        GetComponent<Camera>().orthographicSize = (float)Screen.height * cellAmount / (Screen.width * 2);
    }

    private void LateUpdate()
    {
        if (followedObject != null)
            transform.position = new Vector3(followedObject.position.x + offset.x, followedObject.position.y + offset.y, offset.z);
    }
}