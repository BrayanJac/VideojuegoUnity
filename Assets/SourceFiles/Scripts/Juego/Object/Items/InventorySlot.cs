using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int cantidad;

    public InventorySlot(ItemData nuevoItem)
    {
        item = nuevoItem;
        cantidad = 1;
    }
}