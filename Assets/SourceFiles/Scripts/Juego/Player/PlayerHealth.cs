using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    private float saludMaxima = 100f;
    private float saludActual;

    [Header("UI")]
    public Image barraVida;
    public TMP_Text textoPorcentaje;

    [Header("Referencias")]
    private static PlayerHealth instance;

    public static PlayerHealth Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        saludActual = saludMaxima;
    }

    private void Start()
    {
        ActualizarUI();
    }

    public void RecibirDanio(float cantidad)
    {
        saludActual -= cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarUI();

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    public void Curar(float cantidad)
    {
        saludActual += cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarUI();
    }

    public float ObtenerPorcentajeVida()
    {
        return saludActual / saludMaxima;
    }

    private void ActualizarUI()
    {
        float porcentaje = ObtenerPorcentajeVida();

        if (barraVida != null)
        {
            barraVida.fillAmount = porcentaje;

            if (porcentaje > 0.7f)
                barraVida.color = Color.green;
            else if (porcentaje > 0.3f)
                barraVida.color = Color.yellow;
            else
                barraVida.color = Color.red;
        }

        if (textoPorcentaje != null)
        {
            textoPorcentaje.text = Mathf.RoundToInt(porcentaje * 100) + "%";
        }
    }

    private void Morir()
    {
        Debug.Log("Jugador ha muerto");
        // Aquí puedes agregar lógica de game over
    }
}