using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLenguaje : MonoBehaviour
{
    public void Spanish()
    {
        Back();
    }

    public void Back()
    {
        SceneManager.LoadScene("MenuOpciones");
    }
}
