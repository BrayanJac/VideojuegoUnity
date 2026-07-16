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
        if (Keyboard.current.iKey.isPressed)
        {
            if (!panelInventario.activeSelf)
            {
                panelInventario.SetActive(true);

                ActualizarInventario();

                playerMovement.puedeMoverse = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            if (panelInventario.activeSelf)
            {
                panelInventario.SetActive(false);

                playerMovement.puedeMoverse = true;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void ActualizarInventario()
    {
        listaItems.text = "";

        foreach (InventorySlot slot in InventoryManager.Instance.inventario)
        {
            listaItems.text += slot.item.nombre + " x" + slot.cantidad + "\n";
        }
    }

    private void OnEnable()
    {
        InventoryManager.OnInventoryChanged += ActualizarInventario;
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= ActualizarInventario;
    }

    public void CerrarInventario()
    {
        panelInventario.SetActive(false);

        playerMovement.puedeMoverse = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}