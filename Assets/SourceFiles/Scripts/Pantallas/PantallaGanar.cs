using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PantallaGanar : MonoBehaviour
{
    [Header("Textos UI")]
    public TMP_Text textoEstrellas;
    public TMP_Text textoTiempo;
    public TMP_Text textoNPCs;
    public TMP_Text textoIncendios;
    public TMP_Text textoPuntaje;

    public static int npcSalvados;
    public static int npcTotal;
    public static int incendiosExtinguidos;
    public static int incendiosTotales;
    public static float tiempoRestante;
    public static float tiempoMaximo = 180f;

    void Start()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTiempo.text = "Tiempo restante\n" + string.Format("{0:00}:{1:00}", minutos, segundos);

        textoNPCs.text = $"Civiles Salvados\n {npcSalvados} / {npcTotal}";
        textoIncendios.text = $"Incendios Extinguidos\n {incendiosExtinguidos} / {incendiosTotales}";

        float tiempoPorcentaje = tiempoRestante / tiempoMaximo;
        int puntajeBase = 500;
        int puntajeNPCs = npcSalvados * 50;
        int puntajeIncendios = incendiosExtinguidos * 30;
        int puntajeTiempo = Mathf.RoundToInt(tiempoPorcentaje * 200);
        int puntajeTotal = puntajeBase + puntajeNPCs + puntajeIncendios + puntajeTiempo;
        textoPuntaje.text = $"Puntaje\n {puntajeTotal} pts";

        int estrellas;
        if (puntajeTotal >= 1000) estrellas = 5;
        else if (puntajeTotal >= 750) estrellas = 4;
        else if (puntajeTotal >= 500) estrellas = 3;
        else if (puntajeTotal >= 250) estrellas = 2;
        else estrellas = 1;

        textoEstrellas.text = estrellas + " / 5 Estrellas";
    }

    public void JugarDeNuevo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Juego");
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
