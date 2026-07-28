using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("MenuCargando");
    }

    public void Options()
    {
        SceneManager.LoadScene("MenuOpciones");
    }

    public void Credits()
    {
        SceneManager.LoadScene("MenuCreditos");
    }

    public void Exit()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}
