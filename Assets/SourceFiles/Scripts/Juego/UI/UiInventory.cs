using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public GameObject panelInventario;
    public TextMeshProUGUI listaItems;
    public PlayerMovement playerMovement;

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (panelInventario.activeSelf)
            {
                panelInventario.SetActive(false);
                playerMovement.puedeMoverse = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                panelInventario.SetActive(true);
                MostrarInstrucciones();
                playerMovement.puedeMoverse = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void MostrarInstrucciones()
    {
        listaItems.text =
            "W A S D  -  Moverse\n" +
            "Espacio  -  Saltar\n" +
            "Q        -  Menú de objetos\n" +
            "E        -  Interactuar / Recoger\n" +
            "I        -  Instrucciones\n" +
            "P        -  Pausa";
    }

    public void CerrarInventario()
    {
        panelInventario.SetActive(false);

        playerMovement.puedeMoverse = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
