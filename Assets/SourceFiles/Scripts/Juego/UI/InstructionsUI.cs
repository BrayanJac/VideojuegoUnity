using UnityEngine;
using UnityEngine.InputSystem;

public class InstructionsUI : MonoBehaviour
{
    public GameObject panelInstrucciones;
    public PlayerMovement playerMovement;

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (panelInstrucciones.activeSelf)
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
        panelInstrucciones.SetActive(true);
        
        playerMovement.puedeMoverse = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CerrarInstrucciones()
    {
        panelInstrucciones.SetActive(false);

        playerMovement.puedeMoverse = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}