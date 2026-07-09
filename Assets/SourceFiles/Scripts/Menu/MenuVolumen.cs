using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuVolumen : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderEfectos;


    void Start()
    {
        if (AudioManager.instancia == null)
        {
            Debug.LogError("No existe AudioManager");
            return;
        }

        // Escuchar cambios
        sliderMusica.onValueChanged.AddListener(CambiarMusica);
        sliderEfectos.onValueChanged.AddListener(CambiarEfectos);

        // Cargar volúmenes guardados
        float volumenMusica = PlayerPrefs.GetFloat("MusicaVolumen", 0.5f);
        float volumenEfectos = PlayerPrefs.GetFloat("EfectosVolumen", 0.5f);


        // Mostrar valores en los Slider
        sliderMusica.value = volumenMusica;
        sliderEfectos.value = volumenEfectos;


        Debug.Log("2Musica guardada: " + PlayerPrefs.GetFloat("MusicaVolumen"));
        Debug.Log("2Efectos guardados: " + PlayerPrefs.GetFloat("EfectosVolumen"));
    }


    public void CambiarMusica(float valor)
    {
        AudioManager.instancia.CambiarVolumenMusica(valor);
    }


    public void CambiarEfectos(float valor)
    {
        AudioManager.instancia.CambiarVolumenEfectos(valor);
    }


    public void Back()
    {
        SceneManager.LoadScene("MenuOpciones");
    }
}