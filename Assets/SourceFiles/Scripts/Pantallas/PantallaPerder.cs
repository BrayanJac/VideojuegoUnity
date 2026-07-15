using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaPerder : MonoBehaviour
{
    public void Juego()
    {
        SceneManager.LoadScene("Juego");
    }

    public void Back()
    {

        SceneManager.LoadScene("MenuPrincipal");
    }
}
