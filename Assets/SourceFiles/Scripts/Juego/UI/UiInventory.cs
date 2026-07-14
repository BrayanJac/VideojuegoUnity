using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public GameObject panelInventario;
    public TextMeshProUGUI listaItems;

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            panelInventario.SetActive(!panelInventario.activeSelf);

            if (panelInventario.activeSelf)
            {
                ActualizarInventario();
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
}