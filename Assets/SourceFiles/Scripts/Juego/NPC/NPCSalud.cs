using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCSalud : MonoBehaviour
{
    [Header("Salud")]
    private float saludMaxima = 100f;
    private float saludActual;

    [Header("Deterioro")]
    [Tooltip("Cada cuantos segundos debe perder el porcentaje indicado.")]
    private float tiempoEntreDanio = 20f;

    [Tooltip("Porcentaje de vida que perderá en ese tiempo.")]
    private float porcentajeDanio = 10f;

    [Header("UI")]
    public Canvas canvasBarra;
    public Image barraVida;
    public TMP_Text textoPorcentaje;
    public GameObject alertaCritica;

    [Header("Jugador")]
    public Transform jugador;
    private float distanciaMostrar = 8f;

    private float porcentajeVida;

    // Daño progresivo por segundo
    private float danioPorSegundo;

    // Triángulo
    private bool visible = true;
    private float tiempoParpadeo;
    private Vector3 posicionInicial;

    void Start()
    {
        saludActual = saludMaxima;

        // Calcula cuánto daño perderá cada segundo.
        danioPorSegundo =
            (saludMaxima * (porcentajeDanio / 100f)) / tiempoEntreDanio;

        if (alertaCritica != null)
        {
            posicionInicial = alertaCritica.transform.localPosition;
            alertaCritica.SetActive(false);
        }

        ActualizarUI();
    }

    void Update()
    {
        //---------------------------------
        // VIDA PROGRESIVA
        //---------------------------------

        saludActual -= danioPorSegundo * Time.deltaTime;

        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);

        if (saludActual <= 0)
        {
            Morir();
            return;
        }

        //---------------------------------
        // ACTUALIZAR UI
        //---------------------------------

        ActualizarUI();

        //---------------------------------
        // MOSTRAR BARRA
        //---------------------------------

        if (canvasBarra != null && jugador != null)
        {
            if (porcentajeVida <= 0.5f)
            {
                canvasBarra.enabled = true;
            }
            else
            {
                float distancia =
                    Vector3.Distance(transform.position, jugador.position);

                canvasBarra.enabled = distancia <= distanciaMostrar;
            }
        }

        //---------------------------------
        // ALERTA
        //---------------------------------

        if (alertaCritica != null)
        {
            if (porcentajeVida <= 0.5f)
            {
                if (!alertaCritica.activeSelf)
                    alertaCritica.SetActive(true);

                // Movimiento flotante
                Vector3 pos = posicionInicial;
                pos.y += Mathf.Sin(Time.time * 4f) * 6f;
                alertaCritica.transform.localPosition = pos;

                // Parpadeo
                if (porcentajeVida <= 0.3f)
                {
                    tiempoParpadeo += Time.deltaTime;

                    if (tiempoParpadeo >= 0.35f)
                    {
                        visible = !visible;
                        alertaCritica.SetActive(visible);
                        tiempoParpadeo = 0f;
                    }
                }
                else
                {
                    visible = true;

                    if (!alertaCritica.activeSelf)
                        alertaCritica.SetActive(true);
                }
            }
            else
            {
                alertaCritica.SetActive(false);
            }
        }
    }

    void ActualizarUI()
    {
        porcentajeVida = saludActual / saludMaxima;

        //-----------------------------
        // Barra
        //-----------------------------

        if (barraVida != null)
        {
            barraVida.fillAmount = porcentajeVida;

            if (porcentajeVida > 0.7f)
                barraVida.color = Color.green;
            else if (porcentajeVida > 0.3f)
                barraVida.color = Color.yellow;
            else
                barraVida.color = Color.red;
        }

        //-----------------------------
        // Texto
        //-----------------------------

        if (textoPorcentaje != null)
        {
            textoPorcentaje.text =
                Mathf.CeilToInt(saludActual).ToString() + "%";

            textoPorcentaje.gameObject.SetActive(porcentajeVida <= 0.5f);
        }
    }

    public void RecibirDanio(float cantidad)
    {
        saludActual -= cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarUI();
    }

    public void Curar(float cantidad)
    {
        saludActual += cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarUI();
    }

    void Morir()
    {
        Debug.Log("NPC muerto");
        Destroy(gameObject);
    }
}