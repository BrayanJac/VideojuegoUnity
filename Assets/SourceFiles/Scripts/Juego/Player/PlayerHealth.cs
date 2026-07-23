using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    private float saludMaxima = 100f;

    [Header("UI (configurar en la escena)")]
    [SerializeField] private Canvas canvasBarra;
    [SerializeField] private Image barraVida;
    [SerializeField] private TMP_Text textoPorcentaje;

    [Header("Oxígeno")]
    private float oxigenoMaximo = 100f;
    private float consumoOxigenoPorSegundo = 3f;
    private float danioSinOxigeno = 10f;
    [SerializeField] private Image barraOxigeno;
    [SerializeField] private TMP_Text textoOxigeno;
    private float velocidadAnimacionBarra = 3f;

    [Header("Game Over")]
    private string mensajeSinVida = "Te quedaste sin vida";
    [SerializeField] private string escenaPerder = "PantallaPerder";

    private float saludActual;
    private float oxigenoActual;
    private float oxigenoSuavizado;
    private bool enZonaHumo;
    private float contadorDanioOxigeno;
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
        oxigenoActual = oxigenoMaximo;
        oxigenoSuavizado = oxigenoMaximo;
    }

    private void Start()
    {
        ActualizarUI();
        ActualizarUIOxigeno();
    }

    private void Update()
    {
        if (muerto)
            return;

        if (enZonaHumo)
        {
            oxigenoActual -= consumoOxigenoPorSegundo * Time.deltaTime;
            oxigenoActual = Mathf.Clamp(oxigenoActual, 0, oxigenoMaximo);

            if (oxigenoActual <= 0)
            {
                contadorDanioOxigeno += Time.deltaTime;
                if (contadorDanioOxigeno >= 1f)
                {
                    contadorDanioOxigeno = 0f;
                    RecibirDanio(danioSinOxigeno);
                }
            }
            else
            {
                contadorDanioOxigeno = 0f;
            }
        }

        oxigenoSuavizado = Mathf.Lerp(oxigenoSuavizado, oxigenoActual, velocidadAnimacionBarra * Time.deltaTime);
        ActualizarUIOxigeno();
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

    private void ActualizarUIOxigeno()
    {
        float porcentaje = oxigenoSuavizado / oxigenoMaximo;

        if (barraOxigeno != null)
            barraOxigeno.fillAmount = porcentaje;

        float porcentajeReal = oxigenoActual / oxigenoMaximo;
        if (textoOxigeno != null)
            textoOxigeno.text = Mathf.RoundToInt(porcentajeReal * 100) + "%";
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.StartsWith("vfx_Smoke"))
            enZonaHumo = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.StartsWith("vfx_Smoke"))
        {
            enZonaHumo = false;
            contadorDanioOxigeno = 0f;
        }
    }
}
