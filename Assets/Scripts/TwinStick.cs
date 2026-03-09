using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwinStick : MonoBehaviour
{

    public float speed = 5f;
    public Vector2 movement;
    public Vector2 look;
    public AudioSource SFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)movement * speed * Time.deltaTime;
        //transform.position = movement;
        transform.Rotate(0, 0, look.x * 90 * Time.deltaTime);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack" + context.phase);
        if (context.performed == true)
        {
            SFX.Play();
        }
    }
}
