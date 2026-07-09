using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSonido : MonoBehaviour
{
    public Toggle toggleMusica;
    public Toggle toggleEfectos;


    void Start()
    {
        if (AudioManager.instancia == null)
        {
            Debug.LogError("No existe AudioManager");
            return;
        }

        // Cargar estados guardados
        bool musicaActiva = PlayerPrefs.GetInt("MusicaMute", 0) == 0;
        bool efectosActivos = PlayerPrefs.GetInt("EfectosMute", 0) == 0;


        // Mostrar estado en los Toggle
        toggleMusica.isOn = musicaActiva;
        toggleEfectos.isOn = efectosActivos;


        // Escuchar cambios
        toggleMusica.onValueChanged.AddListener(ActivarMusica);
        toggleEfectos.onValueChanged.AddListener(ActivarEfectos);
    }


    public void ActivarMusica(bool activo)
    {
        AudioManager.instancia.ActivarMusica(activo);
    }


    public void ActivarEfectos(bool activo)
    {
        AudioManager.instancia.ActivarEfectos(activo);
    }


    public void Back()
    {
        SceneManager.LoadScene("MenuOpciones");
    }
}