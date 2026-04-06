using UnityEngine;

//This script controls camera movement.
//It allows the camera to follow the rocket vertically and return to its original position when the rocket is destroyed.

public class CameraFollow : MonoBehaviour
{
    private Transform target;            //The object the camera will follow
    private Vector3 originalPosition;   //The starting position of the camera
    private bool isFollowing = false;   //Tracks whether the camera is actively following a target

    void Start()
    {
        //Store the initial camera position to reset later
        originalPosition = transform.position;
    }

    void LateUpdate()
    {
        //Update camera position after all the movement has occurred
        if (isFollowing && target != null)
        {
            Vector3 newPos = transform.position; //Keep current position
            newPos.y = target.position.y;        //Match only the vertical position of the target
            transform.position = newPos;         //Apply updated position
        }
    }

    //Starts following the target, which is the chosen rocket
    public void FollowTarget(Transform newTarget)
    {
        target = newTarget;   //Assign the right target
        isFollowing = true;  //Enable the follow behavior
    }

    //Stops following and resets camera position
    public void StopFollowing()
    {
        isFollowing = false;   //Disable the follow behavior
        target = null;        //Clear the target reference

        transform.position = originalPosition; //Reset the camera to the initial position
    }

    //Function used by the UnitiEvent to follow the rocket GameObject
    public void FollowRocket(GameObject rocket)
    {
        FollowTarget(rocket.transform);
    }
}