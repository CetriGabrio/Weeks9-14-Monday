using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunRotation : MonoBehaviour
{
    public Vector2 look;
    public GameObject parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = movement;
        transform.Rotate(0, 0, look.x * 180 * Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
}
