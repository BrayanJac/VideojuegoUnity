using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PantallaGanar : MonoBehaviour
{
    [Header("Estrellas")]
    public Image[] estrellasAmarillas;
    public Image[] estrellasTransparentes;

    [Header("Textos UI")]
    public TMP_Text textoTiempo;
    public TMP_Text textoNPCs;
    public TMP_Text textoIncendios;
    public TMP_Text textoPuntaje;

    public static int npcSalvados;
    public static int npcTotal;
    public static int incendiosExtinguidos;
    public static int incendiosTotales;
    public static float tiempoRestante;

    void Start()
    {
        LinternaController.Reset();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);

        textoNPCs.text = $"{npcSalvados} / {npcTotal}";
        textoIncendios.text = $"{incendiosExtinguidos} / {incendiosTotales}";

        float tiempoPorcentaje = DatosDificultad.tiempoMaximo > 0f
            ? Mathf.Clamp01(tiempoRestante / DatosDificultad.tiempoMaximo)
            : 0f;
        int puntajeBase = 500;
        int puntajeNPCs = npcSalvados * 50;
        int puntajeIncendios = incendiosExtinguidos * 30;
        int puntajeTiempo = Mathf.RoundToInt(tiempoPorcentaje * 200);
        int puntajeTotal = puntajeBase + puntajeNPCs + puntajeIncendios + puntajeTiempo;
        textoPuntaje.text = $"{puntajeTotal} pts";

        int estrellas;
        if (puntajeTotal >= 1000) estrellas = 3;
        else if (puntajeTotal >= 600) estrellas = 2;
        else estrellas = 1;

        for (int i = 0; i < 3; i++)
        {
            bool activa = i < estrellas;
            if (i < estrellasAmarillas.Length)
                estrellasAmarillas[i].gameObject.SetActive(activa);
            if (i < estrellasTransparentes.Length)
                estrellasTransparentes[i].gameObject.SetActive(!activa);
        }
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
