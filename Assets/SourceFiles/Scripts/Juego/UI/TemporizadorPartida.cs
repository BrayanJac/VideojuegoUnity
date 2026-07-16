using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TemporizadorPartida : MonoBehaviour
{
    [Header("Tiempo en segundos")]
    private float tiempoInicial = 60f;

    public TMP_Text textoTiempo;

    private float tiempoRestante;
    private bool terminado = false;

    public PantallaPerder pantallaPerder;

    void Start()
    {
        tiempoRestante = tiempoInicial;
    }

    void Update()
    {
        if (terminado)
            return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            terminado = true;

            FinDelTiempo();
        }

        MostrarTiempo();
    }

    void MostrarTiempo()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);

        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void FinDelTiempo()
    {
        PantallaPerder.motivoPerder = "Se te acabó el tiempo";
        SceneManager.LoadScene("PantallaPerder");
    }
}