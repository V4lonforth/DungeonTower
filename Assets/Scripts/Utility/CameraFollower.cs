using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Vector3 offset;
    public Transform followedObject;

    private void LateUpdate()
    {
        if (followedObject)
            transform.position = followedObject.position + offset;
    }
}