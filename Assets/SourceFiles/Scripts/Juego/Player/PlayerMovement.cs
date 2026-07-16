using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 40f;
    private float mouseSensitivity = 0.1f;
    private float gravity = -9.81f;
    private float jumpHeight = 0.5f;

    public Transform playerCamera;
    public bool puedeMoverse = true;

    private CharacterController controller;

    private float verticalVelocity;
    private float cameraPitch = 0f;



    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log(moveSpeed);
    }



    void Update()
    {
        if (!puedeMoverse)
            return;


        // ===== CAMARA =====

        Vector2 mouse = Mouse.current.delta.ReadValue();


        float mouseX = mouse.x * mouseSensitivity;
        float mouseY = mouse.y * mouseSensitivity;


        transform.Rotate(Vector3.up * mouseX);


        cameraPitch -= mouseY;

        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);


        playerCamera.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);



        // ===== MOVIMIENTO =====

        Vector3 move = Vector3.zero;


        if (Keyboard.current.wKey.isPressed)
            move += transform.forward;


        if (Keyboard.current.sKey.isPressed)
            move -= transform.forward;


        if (Keyboard.current.aKey.isPressed)
            move -= transform.right;


        if (Keyboard.current.dKey.isPressed)
            move += transform.right;



        move.Normalize();



        // ===== GRAVEDAD =====

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            // Salto con la barra espaciadora
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        verticalVelocity += gravity * Time.deltaTime;


        move.y = verticalVelocity;


        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}