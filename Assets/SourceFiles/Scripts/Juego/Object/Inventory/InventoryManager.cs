using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public static event Action OnInventoryChanged;

    public List<InventorySlot> inventario = new List<InventorySlot>();

    public InventorySlot objetoEquipado;

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

    private bool EsItemMedico(ItemData item)
    {
        return item.tipoItem == TipoItem.MedKit ||
               item.tipoItem == TipoItem.Analgesico ||
               item.tipoItem == TipoItem.Vendaje ||
               item.tipoItem == TipoItem.Adrenalina;
    }

    public void AgregarItem(ItemData item)
    {
        int cantidadAgregar = EsItemMedico(item) ? 2 : 1;

        // Buscar si el objeto ya existe en el inventario
        foreach (InventorySlot slot in inventario)
        {
            if (slot.item == item)
            {
                if (item.esApilable)
                {
                    slot.cantidad += cantidadAgregar;
                    MostrarInventario();
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }

        // Si no existe, crear un nuevo espacio
        InventorySlot nuevoSlot = new InventorySlot(item);
        nuevoSlot.cantidad = cantidadAgregar;
        inventario.Add(nuevoSlot);

        MostrarInventario();
        OnInventoryChanged?.Invoke();
    }

    private void MostrarInventario()
    {
        Debug.Log("===== INVENTARIO =====");

        foreach (InventorySlot slot in inventario)
        {
            if (slot == null)
            {
                Debug.LogError("Hay un slot vac�o en el inventario");
                continue;
            }

            if (slot.item == null)
            {
                Debug.LogError("Un slot del inventario no tiene ItemData asignado");
                continue;
            }

            Debug.Log(slot.item.nombre + " x" + slot.cantidad);
        }

        if (objetoEquipado != null && objetoEquipado.item != null)
        {
            Debug.Log("Equipado: " + objetoEquipado.item.nombre);
        }

        Debug.Log("======================");
    }

    public void EquiparItem(InventorySlot slot)
    {
        objetoEquipado = slot;

        if (EquipmentManager.Instance != null)
            EquipmentManager.Instance.Equipar(slot.item);

        Debug.Log("Objeto equipado: " + slot.item.nombre);
        OnInventoryChanged?.Invoke();
    }

    public void ConsumirObjetoEquipado()
    {
        if (objetoEquipado == null)
            return;

        objetoEquipado.cantidad--;

        if (objetoEquipado.cantidad <= 0)
        {
            inventario.Remove(objetoEquipado);
            objetoEquipado = null;

            if (EquipmentManager.Instance != null)
                EquipmentManager.Instance.Desequipar();
        }

        OnInventoryChanged?.Invoke();
    }
}