using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Mensajes")]
    public GameObject panelTextoRecoger;
    public TextMeshProUGUI textoRecoger;

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

    public void MostrarTextoRecoger(string nombreObjeto)
    {
        textoRecoger.text = "Presiona [E] para recoger " + nombreObjeto;

        panelTextoRecoger.SetActive(true);
    }

    public void OcultarTextoRecoger()
    {
        panelTextoRecoger.SetActive(false);
    }
}