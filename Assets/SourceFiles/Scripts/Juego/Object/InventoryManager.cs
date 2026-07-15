using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public static event Action OnInventoryChanged;

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
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }

        // Si no existe, crear un nuevo espacio
        inventario.Add(new InventorySlot(item));

        MostrarInventario();
        OnInventoryChanged?.Invoke();
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