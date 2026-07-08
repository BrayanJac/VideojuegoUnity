using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOpciones : MonoBehaviour
{
    public void Sound()
    {
        SceneManager.LoadScene("MenuSonido");
    }

    public void Volume()
    {
        SceneManager.LoadScene("MenuVolumen");
    }

    public void Difficulty()
    {
        SceneManager.LoadScene("MenuDifficultad");
    }

    public void Language()
    {
        SceneManager.LoadScene("MenuLanguage");
    }

    public void Back()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
