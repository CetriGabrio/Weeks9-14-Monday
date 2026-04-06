using UnityEngine;

//This script handles playing and stopping the rocket launch sound.
//It is triggered by UnityEvents to keep audio logic separate from gameplay logic.

public class RocketSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; //Reference to the AudioSource component used for playing the sound

    //Plays the launch sound, which is only called when the rocket is launched, and not instantiated
    public void PlayLaunchSound()
    {
        audioSource.Play();
    }

    //Stops the launch sound, called when the rocket is destroyed to avoid overlapping with the new sound
    public void StopLaunchSound()
    {
        audioSource.Stop();
    }
}