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
    private float distanciaMostrar = 16f;

    private float porcentajeVida;
    public float PorcentajeVida => porcentajeVida;

    private float danioPorSegundo;
    private float multiplicadorDeterioro = 1f;
    private bool deterioroActivo = true;
    private bool notificandoMuerte;

    private bool visible = true;
    private float tiempoParpadeo;
    private Vector3 posicionInicial;

    void Start()
    {
        if (DatosDificultad.nombreDificultad == "Dificil")
            saludActual = saludMaxima * 0.8f;
        else if (DatosDificultad.nombreDificultad == "Normal")
            saludActual = saludMaxima * 0.9f;
        else
            saludActual = saludMaxima;

        danioPorSegundo =
            (saludMaxima * (porcentajeDanio / 100f)) / tiempoEntreDanio;

        multiplicadorDeterioro = DatosDificultad.multiplicadorDeterioroNPC;

        if (alertaCritica != null)
        {
            posicionInicial = alertaCritica.transform.localPosition;
            alertaCritica.SetActive(false);
        }

        ActualizarUI();
    }

    void Update()
    {
        if (deterioroActivo)
        {
            saludActual -= danioPorSegundo * multiplicadorDeterioro * Time.deltaTime;
            saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);

            if (saludActual <= 0)
            {
                Morir();
                return;
            }
        }

        ActualizarUI();
        ActualizarVisibilidadUI();
        ActualizarAlerta();
    }

    void ActualizarVisibilidadUI()
    {
        bool mostrar = false;

        NPCHerido herido = GetComponent<NPCHerido>();
        if (herido != null && herido.EstaRescatado)
        {
            if (canvasBarra != null)
                canvasBarra.enabled = false;
            if (textoPorcentaje != null)
                textoPorcentaje.gameObject.SetActive(false);
            if (alertaCritica != null)
                alertaCritica.SetActive(false);
            return;
        }

        if (jugador != null)
        {
            if (porcentajeVida <= 0.5f)
            {
                mostrar = true;
            }
            else
            {
                float distancia =
                    Vector3.Distance(transform.position, jugador.position);
                mostrar = distancia <= distanciaMostrar;
            }
        }
        else if (porcentajeVida <= 0.5f)
        {
            mostrar = true;
        }

        if (canvasBarra != null)
            canvasBarra.enabled = mostrar;

        if (textoPorcentaje != null)
            textoPorcentaje.gameObject.SetActive(mostrar);
    }

    void ActualizarAlerta()
    {
        if (alertaCritica == null)
            return;

        if (porcentajeVida <= 0.5f && deterioroActivo)
        {
            if (!alertaCritica.activeSelf)
                alertaCritica.SetActive(true);

            Vector3 pos = posicionInicial;
            pos.y += Mathf.Sin(Time.time * 4f) * 6f;
            alertaCritica.transform.localPosition = pos;

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

    void ActualizarUI()
    {
        porcentajeVida = saludActual / saludMaxima;

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

        if (textoPorcentaje != null)
        {
            if (porcentajeVida <= 0)
                textoPorcentaje.text = "MUERTO";
            else
                textoPorcentaje.text = Mathf.RoundToInt(porcentajeVida * 100) + "%";
        }
    }

    public void RecibirDanio(float cantidad)
    {
        if (!deterioroActivo)
            return;

        saludActual -= cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarUI();

        if (saludActual <= 0)
            Morir();
    }

    public void Curar(float cantidad)
    {
        saludActual += cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarUI();

        if (EstaCurado())
            DetenerDeterioro();
    }

    public void DetenerDeterioro()
    {
        deterioroActivo = false;
        multiplicadorDeterioro = 1f;
        ActualizarUI();
    }

    public void EstablecerMultiplicadorDeterioro(float multiplicador)
    {
        multiplicadorDeterioro = Mathf.Max(0f, multiplicador);
    }

    public void Morir()
    {
        if (notificandoMuerte)
            return;

        notificandoMuerte = true;
        deterioroActivo = false;
        saludActual = 0f;
        ActualizarUI();

        Debug.Log("NPC muerto");

        NPCHerido herido = GetComponent<NPCHerido>();
        herido?.NotificarMuerte();
    }

    public bool EstaCurado()
    {
        return saludActual >= saludMaxima;
    }
}