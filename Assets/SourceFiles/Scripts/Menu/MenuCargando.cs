using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuCargando : MonoBehaviour
{
    public TMP_Text textoMision;
    public GameObject panelContinuar;

    private AsyncOperation operacion;
    private bool cargaCompleta;

    private string mision =
        "Recoge suministros médicos, apaga los incendios, rescata a todos los civiles heridos y llévalos a la ambulancia antes de que sea demasiado tarde.";

    void Start()
    {
        if (textoMision != null)
            textoMision.text = mision;

        if (panelContinuar != null)
            panelContinuar.SetActive(false);

        operacion = SceneManager.LoadSceneAsync("Juego");
        operacion.allowSceneActivation = false;
    }

    void Update()
    {
        if (operacion == null) return;

        if (!cargaCompleta && operacion.progress >= 0.9f)
        {
            cargaCompleta = true;

            if (panelContinuar != null)
                panelContinuar.SetActive(true);
        }

        if (cargaCompleta &&
            (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame))
        {
            operacion.allowSceneActivation = true;
        }
    }
}