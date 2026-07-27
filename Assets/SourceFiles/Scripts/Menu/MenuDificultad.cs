using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDificultad : MonoBehaviour
{
    public void Easy()
    {
        DatosDificultad.tiempoMaximo = 300f;
        DatosDificultad.multiplicadorDeterioroNPC = 0.5f;
        DatosDificultad.nombreDificultad = "Facil";
        Back();
    }

    public void Medium()
    {
        DatosDificultad.tiempoMaximo = 180f;
        DatosDificultad.multiplicadorDeterioroNPC = 1f;
        DatosDificultad.nombreDificultad = "Normal";
        Back();
    }

    public void Hard()
    {
        DatosDificultad.tiempoMaximo = 120f;
        DatosDificultad.multiplicadorDeterioroNPC = 2f;
        DatosDificultad.nombreDificultad = "Dificil";
        Back();
    }

    public void Back()
    {
        SceneManager.LoadScene("MenuOpciones");
    }
}