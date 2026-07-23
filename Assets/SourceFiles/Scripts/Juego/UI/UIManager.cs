using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Mensajes")]
    public GameObject panelTextoRecoger;
    public TextMeshProUGUI textoRecoger;

    [Header("HUD Rescates (arriba izquierda)")]
    public TextMeshProUGUI textoRescates;
    public Image iconoRescates;

    [Header("HUD Equipado (abajo derecha)")]
    public GameObject panelEquipado;
    public TextMeshProUGUI textoEquipado;
    public Image iconoEquipado;

    [Header("HUD Linterna (arriba centro)")]
    public GameObject iconoLinterna;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (ContadorRescates.Instance != null)
            ActualizarRescates(ContadorRescates.Instance.Salvados, ContadorRescates.Instance.Total);

        ActualizarEquipado();
        ActualizarIconoLinterna();
    }

    private void OnEnable()
    {
        ContadorRescates.OnRescatesChanged += ActualizarRescates;
        InventoryManager.OnInventoryChanged += ActualizarEquipado;
    }

    private void OnDisable()
    {
        ContadorRescates.OnRescatesChanged -= ActualizarRescates;
        InventoryManager.OnInventoryChanged -= ActualizarEquipado;
    }

    public void MostrarTextoRecoger(string nombreObjeto)
    {
        MostrarTextoAccion("recoger " + nombreObjeto);
    }

    public void MostrarTextoAccion(string accion)
    {
        if (textoRecoger == null || panelTextoRecoger == null)
            return;

        textoRecoger.text = "Presiona [E] para " + accion;
        panelTextoRecoger.SetActive(true);
    }

    public void OcultarTextoRecoger()
    {
        if (panelTextoRecoger != null)
            panelTextoRecoger.SetActive(false);
    }

    private void ActualizarRescates(int salvados, int total)
    {
        if (textoRescates != null)
            textoRescates.text = $"{salvados}/{total}";
    }

    private void ActualizarEquipado()
    {
        if (panelEquipado == null || textoEquipado == null || iconoEquipado == null)
            return;

        InventorySlot equipado = InventoryManager.Instance != null
            ? InventoryManager.Instance.objetoEquipado
            : null;

        if (equipado == null || equipado.item == null)
        {
            panelEquipado.SetActive(true);
            textoEquipado.text = "Sin equipo";
            iconoEquipado.enabled = false;
            return;
        }

        panelEquipado.SetActive(true);
        iconoEquipado.enabled = true;
        iconoEquipado.sprite = equipado.item.icono;
        iconoEquipado.color = Color.white;

        string cantidad = equipado.cantidad > 1 ? $" x{equipado.cantidad}" : "";
        textoEquipado.text = equipado.item.nombre + cantidad;
    }

    public void ActualizarIconoLinterna()
    {
        if (iconoLinterna != null)
            iconoLinterna.SetActive(LinternaController.linternaRecogida);
    }
}
