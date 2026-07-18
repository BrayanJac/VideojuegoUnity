using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    public GameObject imgPausa;
    public InventoryUI inventoryUI;
    public RadialMenu radialMenu;

    private bool pausado = false;


    void Start()
    {
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

        Debug.Log("inventoryUI: " + inventoryUI);
        Debug.Log("radialMenu: " + radialMenu);
        Debug.Log("imgPausa: " + imgPausa);

        pausado = true;

        inventoryUI.CerrarInventario();
        radialMenu.CerrarMenu();
        imgPausa.SetActive(true);

        Time.timeScale = 0f;

        // Liberar mouse para los botones del menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void Continue()
    {
        Debug.Log("JUEGO CONTINUADO");

        pausado = false;

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