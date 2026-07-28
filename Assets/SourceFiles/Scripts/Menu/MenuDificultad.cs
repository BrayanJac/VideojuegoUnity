using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuDificultad : MonoBehaviour
{
    public Button btnEasy;
    public Button btnMedium;
    public Button btnHard;
    public TextMeshProUGUI textoSeleccion;

    private ColorBlock coloresNormal;
    private ColorBlock coloresSeleccionado;

    private void Start()
    {
        coloresNormal = btnEasy.colors;
        coloresSeleccionado = coloresNormal;
        coloresSeleccionado.normalColor = Color.green;
        coloresSeleccionado.selectedColor = Color.green;

        ActualizarSeleccion();
    }

    private void ActualizarSeleccion()
    {
        if (textoSeleccion != null)
            textoSeleccion.text = "Dificultad: " + DatosDificultad.nombreDificultad;

        btnEasy.colors = DatosDificultad.nombreDificultad == "Facil" ? coloresSeleccionado : coloresNormal;
        btnMedium.colors = DatosDificultad.nombreDificultad == "Normal" ? coloresSeleccionado : coloresNormal;
        btnHard.colors = DatosDificultad.nombreDificultad == "Dificil" ? coloresSeleccionado : coloresNormal;
    }

    public void Easy()
    {
        DatosDificultad.tiempoMaximo = 300f;
        DatosDificultad.multiplicadorDeterioroNPC = 0.5f;
        DatosDificultad.nombreDificultad = "Facil";
        ActualizarSeleccion();
    }

    public void Medium()
    {
        DatosDificultad.tiempoMaximo = 180f;
        DatosDificultad.multiplicadorDeterioroNPC = 1f;
        DatosDificultad.nombreDificultad = "Normal";
        ActualizarSeleccion();
    }

    public void Hard()
    {
        DatosDificultad.tiempoMaximo = 120f;
        DatosDificultad.multiplicadorDeterioroNPC = 2f;
        DatosDificultad.nombreDificultad = "Dificil";
        ActualizarSeleccion();
    }

    public void Back()
    {
        SceneManager.LoadScene("MenuOpciones");
    }
}