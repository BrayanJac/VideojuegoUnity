using UnityEngine;
using UnityEngine.InputSystem;

public class InstructionsUI : MonoBehaviour
{
    public GameObject imgControles;
    public GameObject fondoPausa;
    public PlayerMovement playerMovement;
    public GameObject crosshair;

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (imgControles.activeSelf)
            {
                CerrarInstrucciones();
            }
            else
            {
                AbrirInstrucciones();
            }
        }
    }

    void AbrirInstrucciones()
    {
        fondoPausa.SetActive(true);
        imgControles.SetActive(true);

        if (crosshair != null) crosshair.SetActive(false);

        playerMovement.puedeMoverse = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CerrarInstrucciones()
    {
        fondoPausa.SetActive(false);
        imgControles.SetActive(false);

        if (crosshair != null) crosshair.SetActive(true);

        playerMovement.puedeMoverse = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}