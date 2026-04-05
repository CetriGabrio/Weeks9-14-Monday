using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float speed = 5f;
    private bool hasLaunched = false;
    public ParticleSystem flameParticles;

    void Start()
    {
        if (flameParticles != null)
        {
            flameParticles.Stop();
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void Launch()
    {
        hasLaunched = true;

        if (flameParticles != null)
        {
            flameParticles.Play();
        }
    }

    void Update()
    {
        if (hasLaunched)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}