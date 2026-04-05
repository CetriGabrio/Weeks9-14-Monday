using UnityEngine;

public class RocketSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayLaunchSound()
    {
        audioSource.Play();
    }

    public void StopLaunchSound()
    {
        audioSource.Stop();
    }
}