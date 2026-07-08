using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCamera : MonoBehaviour
{
    public float speed = 10f;
    public float mouseSensitivity = 0.1f;

    float rotX;
    float rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Ratón
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        rotX += mouseDelta.x * mouseSensitivity;
        rotY -= mouseDelta.y * mouseSensitivity;

        rotY = Mathf.Clamp(rotY, -80f, 80f);

        transform.rotation = Quaternion.Euler(rotY, rotX, 0);

        // Movimiento
        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            move += transform.forward;

        if (Keyboard.current.sKey.isPressed)
            move -= transform.forward;

        if (Keyboard.current.dKey.isPressed)
            move += transform.right;

        if (Keyboard.current.aKey.isPressed)
            move -= transform.right;

        if (Keyboard.current.eKey.isPressed)
            move += Vector3.up;

        if (Keyboard.current.qKey.isPressed)
            move += Vector3.down;

        transform.position += move.normalized * speed * Time.deltaTime;
    }
}