using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 originalPosition;
    private bool isFollowing = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void LateUpdate()
    {
        if (isFollowing && target != null)
        {
            Vector3 newPos = transform.position;
            newPos.y = target.position.y;
            transform.position = newPos;
        }
    }

    public void FollowTarget(Transform newTarget)
    {
        target = newTarget;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
        target = null;

        transform.position = originalPosition;
    }
}