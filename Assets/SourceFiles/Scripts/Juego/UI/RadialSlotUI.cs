using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icono;
    public Image fondo;
    public Image borde;
    public Image sombra;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI cantidadText;

    public InventorySlot slot;
    public bool seleccionado = false;

    [Header("Animacion")]
    public float duracionAnimacion = 0.2f;
    public float escalaSeleccionado = 1.15f;

    [Header("Colores")]
    public Color colorBordeSeleccionado = new Color(0.3f, 0.6f, 1f, 1f);
    public Color colorBordeNormal = new Color(1f, 1f, 1f, 0.4f);
    public Color colorIconoSeleccionado = Color.white;
    public Color colorIconoNormal = new Color(1f, 1f, 1f, 1f);
    public Color colorTextoSeleccionado = Color.white;
    public Color colorTextoNormal = new Color(1f, 1f, 1f, 1f);

    private Coroutine animacionActual;
    private bool estadoAnterior;
    private Vector3 escalaBase;
    private Color colorBaseIcono;

    void Start()
    {
        escalaBase = transform.localScale;

        if (icono != null)
            colorBaseIcono = icono.color;

        estadoAnterior = seleccionado;
    }

    void Update()
    {
        if (seleccionado != estadoAnterior)
        {
            if (animacionActual != null)
                StopCoroutine(animacionActual);

            animacionActual = StartCoroutine(AnimarSeleccion(seleccionado));
            estadoAnterior = seleccionado;
        }
    }

    IEnumerator AnimarSeleccion(bool activar)
    {
        float tiempo = 0;

        Vector3 escalaInicial = transform.localScale;
        Vector3 escalaFinal = activar ? escalaBase * escalaSeleccionado : escalaBase;

        Color bordeInicial = borde != null ? borde.color : Color.clear;
        Color bordeFinal = activar ? colorBordeSeleccionado : colorBordeNormal;

        Color iconoInicial = icono != null ? icono.color : Color.white;
        Color iconoFinal = activar ? colorIconoSeleccionado : colorBaseIcono;

        Color textoInicial = nombre != null ? nombre.color : Color.white;
        Color textoFinal = activar ? colorTextoSeleccionado : colorTextoNormal;

        while (tiempo < duracionAnimacion)
        {
            float t = tiempo / duracionAnimacion;
            t = Mathf.SmoothStep(0, 1, t);

            transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);

            if (borde != null)
                borde.color = Color.Lerp(bordeInicial, bordeFinal, t);

            if (icono != null)
                icono.color = Color.Lerp(iconoInicial, iconoFinal, t);

            if (nombre != null)
                nombre.color = Color.Lerp(textoInicial, textoFinal, t);

            if (cantidadText != null)
                cantidadText.color = Color.Lerp(textoInicial, textoFinal, t);

            tiempo += Time.deltaTime;
            yield return null;
        }

        transform.localScale = escalaFinal;
        if (borde != null) borde.color = bordeFinal;
        if (icono != null) icono.color = iconoFinal;
        if (nombre != null) nombre.color = textoFinal;
        if (cantidadText != null) cantidadText.color = textoFinal;

        animacionActual = null;
    }

    public void Configurar(InventorySlot nuevoSlot)
    {
        slot = nuevoSlot;

        if (icono != null)
            icono.sprite = slot.item.icono;

        if (nombre != null)
            nombre.text = slot.item.nombre;

        if (cantidadText != null)
        {
            cantidadText.text = $"x{slot.cantidad}";
            cantidadText.gameObject.SetActive(slot.item.esApilable);
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
