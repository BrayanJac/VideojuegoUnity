using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PantallaPerder : MonoBehaviour
{
    public TMP_Text textoPerder;

    // Aquí se guarda el motivo antes de cargar la escena.
    public static string motivoPerder = "Has perdido";

    void Start()
    {
        textoPerder.text = motivoPerder;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Juego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Juego");
    }

    public void Back()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}