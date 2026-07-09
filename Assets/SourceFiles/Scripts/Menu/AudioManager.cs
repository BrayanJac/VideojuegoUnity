using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instancia;

    public AudioSource musica;
    public AudioSource efectos;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        musica.volume = PlayerPrefs.GetFloat("MusicaVolumen", 0.5f);
        efectos.volume = PlayerPrefs.GetFloat("EfectosVolumen", 0.5f);

        musica.mute = PlayerPrefs.GetInt("MusicaMute", 0) == 1;
        efectos.mute = PlayerPrefs.GetInt("EfectosMute", 0) == 1;

        if (!musica.isPlaying)
        {
            musica.Play();
        }

        Debug.Log("Musica guardada: " + PlayerPrefs.GetFloat("MusicaVolumen"));
        Debug.Log("Efectos guardados: " + PlayerPrefs.GetFloat("EfectosVolumen"));
    }


    public void CambiarVolumenMusica(float volumen)
    {
        musica.volume = volumen;
        PlayerPrefs.SetFloat("MusicaVolumen", volumen);
        PlayerPrefs.Save();
    }


    public void CambiarVolumenEfectos(float volumen)
    {
        efectos.volume = volumen;
        PlayerPrefs.SetFloat("EfectosVolumen", volumen);
        PlayerPrefs.Save();
    }


    public void ActivarMusica(bool activo)
    {
        musica.mute = !activo;
        PlayerPrefs.SetInt("MusicaMute", activo ? 0 : 1);
        PlayerPrefs.Save();
    }


    public void ActivarEfectos(bool activo)
    {
        efectos.mute = !activo;
        PlayerPrefs.SetInt("EfectosMute", activo ? 0 : 1);
        PlayerPrefs.Save();
    }
}