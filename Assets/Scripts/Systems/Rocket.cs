using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float speed = 5f;
    private bool hasLaunched = false;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void Launch()
    {
        hasLaunched = true;
    }

    void Update()
    {
        if (hasLaunched)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}