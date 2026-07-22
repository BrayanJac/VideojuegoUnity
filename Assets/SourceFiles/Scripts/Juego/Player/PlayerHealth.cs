using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    [SerializeField] private float saludMaxima = 100f;

    [Header("UI (configurar en la escena)")]
    [SerializeField] private Canvas canvasBarra;
    [SerializeField] private Image barraVida;
    [SerializeField] private TMP_Text textoPorcentaje;

    [Header("Game Over")]
    [SerializeField] private string mensajeSinVida = "Te quedaste sin vida";
    [SerializeField] private string escenaPerder = "PantallaPerder";

    private float saludActual;
    private static PlayerHealth instance;
    private bool muerto;

    public static PlayerHealth Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
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
        if (muerto)
            return;

        saludActual -= cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);
        ActualizarUI();

        if (saludActual <= 0)
            Morir();
    }

    public void Curar(float cantidad)
    {
        if (muerto)
            return;

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
            textoPorcentaje.text = Mathf.RoundToInt(porcentaje * 100) + "%";
    }

    private void Morir()
    {
        if (muerto)
            return;

        muerto = true;
        saludActual = 0f;
        ActualizarUI();

        PlayerMovement movimiento = GetComponent<PlayerMovement>();
        if (movimiento != null)
            movimiento.puedeMoverse = false;

        PantallaPerder.motivoPerder = mensajeSinVida;
        SceneManager.LoadScene(escenaPerder);
    }
}
