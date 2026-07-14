using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventorySlot> inventario = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AgregarItem(ItemData item)
    {
        // Buscar si el objeto ya existe en el inventario
        foreach (InventorySlot slot in inventario)
        {
            if (slot.item == item)
            {
                if (item.esApilable)
                {
                    slot.cantidad++;
                    MostrarInventario();
                    return;
                }
            }
        }

        // Si no existe, crear un nuevo espacio
        inventario.Add(new InventorySlot(item));

        MostrarInventario();
    }

    private void MostrarInventario()
    {
        Debug.Log("===== INVENTARIO =====");

        foreach (InventorySlot slot in inventario)
        {
            Debug.Log(slot.item.nombre + " x" + slot.cantidad);
        }

        Debug.Log("======================");
    }
}