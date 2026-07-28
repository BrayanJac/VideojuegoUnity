using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public GameObject menu;
    public CanvasGroup canvasGroup;
    public PlayerMovement playerMovement;
    public GameObject slotPrefab;
    public Transform contenedorSlots;

    public float radio = 180f;

    public TMPro.TextMeshProUGUI textoSinObjetos;
    public GameObject crosshair;

    [Header("Animacion")]
    public float duracionApertura = 0.15f;

    private List<GameObject> slots = new List<GameObject>();
    private RadialSlotUI slotSeleccionado;
    private bool menuAbierto = false;
    private Coroutine animacionActual;

    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        menu.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && !menuAbierto)
        {
            AbrirMenu();
        }

        if (Keyboard.current.qKey.wasReleasedThisFrame && menuAbierto)
        {
            CerrarMenu();
        }

        slotSeleccionado = null;

        foreach (GameObject slot in slots)
        {
            if (slot == null) continue;

            RadialSlotUI ui = slot.GetComponent<RadialSlotUI>();

            if (ui != null && ui.seleccionado)
            {
                slotSeleccionado = ui;
            }
        }
    }

    void AbrirMenu()
    {
        if (animacionActual != null)
            StopCoroutine(animacionActual);

        menuAbierto = true;
        menu.SetActive(true);

        CrearSlots();

        animacionActual = StartCoroutine(AnimacionApertura());

        if (crosshair != null) crosshair.SetActive(false);

        if (playerMovement != null)
            playerMovement.puedeMoverse = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator AnimacionApertura()
    {
        float tiempo = 0;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        transform.localScale = Vector3.zero;

        while (tiempo < duracionApertura)
        {
            float t = tiempo / duracionApertura;
            t = Mathf.SmoothStep(0, 1, t);

            if (canvasGroup != null)
                canvasGroup.alpha = t;

            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            tiempo += Time.deltaTime;
            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        transform.localScale = Vector3.one;
        animacionActual = null;
    }

    void CrearSlots()
    {
        foreach (GameObject slot in slots)
        {
            if (slot != null)
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

            if (slotUI != null)
                slotUI.Configurar(InventoryManager.Instance.inventario[i]);

            float anguloActual = i * angulo * Mathf.Deg2Rad;

            float x = Mathf.Cos(anguloActual) * radio;
            float y = Mathf.Sin(anguloActual) * radio;

            RectTransform rect = nuevoSlot.GetComponent<RectTransform>();

            if (rect != null)
                rect.anchoredPosition = new Vector2(x, y);

            slots.Add(nuevoSlot);
        }
    }

    public void CerrarMenu()
    {
        if (animacionActual != null)
            StopCoroutine(animacionActual);

        animacionActual = StartCoroutine(AnimacionCierre());
    }

    IEnumerator AnimacionCierre()
    {
        if (slotSeleccionado != null)
        {
            InventoryManager.Instance.EquiparItem(slotSeleccionado.slot);
        }

        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        float tiempo = 0;
        Vector3 escalaInicial = transform.localScale;

        while (tiempo < duracionApertura)
        {
            float t = tiempo / duracionApertura;
            t = Mathf.SmoothStep(0, 1, t);

            if (canvasGroup != null)
                canvasGroup.alpha = 1f - t;

            transform.localScale = Vector3.Lerp(escalaInicial, Vector3.zero, t);

            tiempo += Time.deltaTime;
            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        transform.localScale = Vector3.zero;

        if (crosshair != null) crosshair.SetActive(true);

        if (playerMovement != null)
            playerMovement.puedeMoverse = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        menu.SetActive(false);
        menuAbierto = false;
        animacionActual = null;
    }
}
