using UnityEngine;

//This script controls the rocket's movement and visual effects.
//It allows the rocket to stay idle in preview mode and move upward when launched.

public class Rocket : MonoBehaviour
{
    private float speed = 5f; //Speed of the rocket
    private bool hasLaunched = false; //Checks whether the rocket has been launched

    public ParticleSystem flameParticles; //Reference to the flame particle effect

    void Start()
    {
        //Ensure that the particles are off while the rocket is in preview mode
        if (flameParticles != null)
        {
            flameParticles.Stop();
        }
    }

    //Sets the speed of the rocket based on the UI slider
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    //Starts the rocket upward movement and the particle effect with it
    public void Launch()
    {
        hasLaunched = true; //Enable the movement

        //Start the flame particle
        if (flameParticles != null)
        {
            flameParticles.Play();
        }
    }

    void Update()
    {
        //Move the rocket upward only after it has been launch
        if (hasLaunched)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}