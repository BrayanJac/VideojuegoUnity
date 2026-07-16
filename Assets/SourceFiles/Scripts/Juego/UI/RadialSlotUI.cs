using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icono;

    public Image fondo;

    public TextMeshProUGUI nombre;

    public InventorySlot slot;

    public bool seleccionado = false;

    void Update()
    {
        if (seleccionado)
        {
            fondo.color = Color.cyan;
        }
        else
        {
            fondo.color = Color.white;
        }
    }

    public void Configurar(InventorySlot nuevoSlot)
    {
        slot = nuevoSlot;

        icono.sprite = slot.item.icono;

        if (slot.item.esApilable)
        {
            nombre.text = $"{slot.item.nombre} x{slot.cantidad}";
        }
        else
        {
            nombre.text = slot.item.nombre;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        seleccionado = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        seleccionado = false;
    }
}