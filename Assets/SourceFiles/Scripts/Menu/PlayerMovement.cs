using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float mouseSensitivity = 0.1f;
    public float gravity = -9.81f;

    public Transform playerCamera;


    private CharacterController controller;

    private float verticalVelocity;
    private float cameraPitch = 0f;



    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    void Update()
    {
        // Si está pausado no mueve jugador ni cámara
        if (Time.timeScale == 0f)
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

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;


        verticalVelocity += gravity * Time.deltaTime;


        move.y = verticalVelocity;


        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}