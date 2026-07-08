using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDificultad : MonoBehaviour
{
    public void Easy()
    {
        
        Back();
    }

    public void Medium()
    {
        
        Back();
    }

    public void Difficullty()
    {

        Back();
    }

    public void Back()
    {
        SceneManager.LoadScene("MenuOpciones");
    }
}
