using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Vector3 offset;
    public Transform followedObject;

    private void LateUpdate()
    {
        if (followedObject)
            transform.position = new Vector3(followedObject.position.x + offset.x, followedObject.position.y + offset.y, offset.z) ;
    }
}