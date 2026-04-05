using UnityEngine;

public class CameraEvent : MonoBehaviour
{
    public RocketLauncherUI launcher;
    public CameraFollow cameraFollow;

    public void FollowCurrentRocket()
    {
        GameObject rocket = launcher.GetCurrentRocket();

        if (rocket != null)
        {
            cameraFollow.FollowRocket(rocket);
        }
    }
}