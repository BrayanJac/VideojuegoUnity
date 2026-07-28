using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadialMenu : MonoBehaviour
{
    public GameObject menu;

    public PlayerMovement playerMovement;

    public GameObject slotPrefab;

    public Transform contenedorSlots;

    public float radio = 180f;

    public TMPro.TextMeshProUGUI textoSinObjetos;

    private List<GameObject> slots = new List<GameObject>();

    private RadialSlotUI slotSeleccionado;

    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            AbrirMenu();
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            CerrarMenu();
        }

        slotSeleccionado = null;

        foreach (GameObject slot in slots)
        {
            RadialSlotUI ui = slot.GetComponent<RadialSlotUI>();

            if (ui.seleccionado)
            {
                slotSeleccionado = ui;
            }
        }
    }

    void AbrirMenu()
    {
        menu.SetActive(true);

        CrearSlots();

        playerMovement.puedeMoverse = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CrearSlots()
    {
        // Eliminar los anteriores
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }

        slots.Clear();

        int cantidad = InventoryManager.Instance.inventario.Count;

        if (cantidad == 0)
        {
            if (textoSinObjetos != null)
                textoSinObjetos.gameObject.SetActive(true);
            return;
        }

        if (textoSinObjetos != null)
            textoSinObjetos.gameObject.SetActive(false);

        float angulo = 360f / cantidad;

        for (int i = 0; i < cantidad; i++)
        {
            GameObject nuevoSlot = Instantiate(slotPrefab, contenedorSlots);

            RadialSlotUI slotUI = nuevoSlot.GetComponent<RadialSlotUI>();

            slotUI.Configurar(InventoryManager.Instance.inventario[i]);

            float anguloActual = i * angulo * Mathf.Deg2Rad;

            float x = Mathf.Cos(anguloActual) * radio;
            float y = Mathf.Sin(anguloActual) * radio;

            RectTransform rect = nuevoSlot.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(x, y);

            slots.Add(nuevoSlot);
        }
    }

    public void CerrarMenu()
    {
        if (slotSeleccionado != null)
        {
            InventoryManager.Instance.EquiparItem(slotSeleccionado.slot);
        }

        menu.SetActive(false);

        playerMovement.puedeMoverse = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}