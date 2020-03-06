using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Vector3 offset;
    public Transform followedObject;

    private void Update()
    {
        if (followedObject)
            transform.position = followedObject.position + offset;
    }
}