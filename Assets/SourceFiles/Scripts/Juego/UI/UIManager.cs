using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Mensajes")]
    public GameObject textoRecoger;

    private void Awake()
    {
        // Solo debe existir un UIManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MostrarTextoRecoger()
    {
        textoRecoger.SetActive(true);
    }

    public void OcultarTextoRecoger()
    {
        textoRecoger.SetActive(false);
    }
}