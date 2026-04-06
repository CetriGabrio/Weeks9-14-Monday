using UnityEngine;

//This script is to connect the rocket launcher and the camera system to allow the UnityEvent to trigger the camera follow using the currently active rocket.

public class CameraEvent : MonoBehaviour
{
    public RocketLauncherUI launcher; //Reference to the rocket launcher to access the current rocket
    public CameraFollow cameraFollow; //Reference to the camera follow system

    //Called by UnityEvent to make the camera follow the current rocket
    public void FollowCurrentRocket()
    {
        GameObject rocket = launcher.GetCurrentRocket(); //Reference only the currently active rocket

        //After the rocket has been instantiated, pass it to the camera system
        if (rocket != null)
        {
            cameraFollow.FollowRocket(rocket);
        }
    }
}