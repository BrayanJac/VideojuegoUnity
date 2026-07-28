using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    public GameObject imgPausa;
    public InstructionsUI instruccionesUI;
    public RadialMenu radialMenu;
    public GameObject fondoPausa;
    public GameObject crosshair;

    private bool pausado = false;


    void Start()
    {
        fondoPausa.SetActive(false);
        imgPausa.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        // Presionar P para pausar/despausar
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (pausado)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Pause()
    {
        Debug.Log("PAUSA ACTIVADA");

        pausado = true;

        instruccionesUI.CerrarInstrucciones();
        radialMenu.CerrarMenu();
        if (crosshair != null) crosshair.SetActive(false);
        fondoPausa.SetActive(true);
        imgPausa.SetActive(true);
        
        Time.timeScale = 0f;

        // Liberar mouse para los botones del men�
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void Continue()
    {
        Debug.Log("JUEGO CONTINUADO");

        pausado = false;

        if (crosshair != null) crosshair.SetActive(true);
        fondoPausa.SetActive(false);
        imgPausa.SetActive(false);

        Time.timeScale = 1f;

        // Volver al modo FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void Menu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuPrincipal");
    }
}