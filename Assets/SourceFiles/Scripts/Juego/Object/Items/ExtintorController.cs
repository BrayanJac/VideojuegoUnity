using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExtintorController : MonoBehaviour
{
    public static int incendiosApagados;
    public static int incendiosTotales;

    private float rangoApagado = 30f;
    [SerializeField] private Transform fuegoEdificio;

    private ParticleSystem[] fuegos;
    private Transform jugador;
    private Camera camaraJugador;
    private GameObject crosshair;
    private TMP_Text textoCrosshair;

    private void Start()
    {
        if (fuegoEdificio == null)
        {
            GameObject fuegoObj = GameObject.Find("FuegoEdificio");
            if (fuegoObj != null)
                fuegoEdificio = fuegoObj.transform;
        }

        if (fuegoEdificio != null)
        {
            fuegos = fuegoEdificio.GetComponentsInChildren<ParticleSystem>();
            incendiosTotales = fuegos.Length;
        }

        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }
        camaraJugador = Camera.main;

        CrearCrosshair();
    }

    void CrearCrosshair()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null) return;

        crosshair = new GameObject("CrosshairExtintor");
        crosshair.transform.SetParent(canvas.transform, false);

        textoCrosshair = crosshair.AddComponent<TextMeshProUGUI>();
        textoCrosshair.text = "✕";
        textoCrosshair.fontSize = 48;
        textoCrosshair.alignment = TextAlignmentOptions.Center;
        textoCrosshair.color = new Color(1, 1, 1, 0);

        RectTransform rt = textoCrosshair.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        if (!EstaExtintorEquipado())
        {
            if (textoCrosshair != null)
                textoCrosshair.color = new Color(1, 1, 1, 0);
            return;
        }

        bool apuntandoFuego = EstaApuntandoFuego();

        if (textoCrosshair != null)
        {
            if (apuntandoFuego)
                textoCrosshair.color = Color.white;
            else
                textoCrosshair.color = new Color(1, 1, 1, 0.5f);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
            IntentarApagarFuegos(apuntandoFuego);
    }

    bool EstaApuntandoFuego()
    {
        if (camaraJugador == null || fuegos == null) return false;

        foreach (ParticleSystem fuego in fuegos)
        {
            if (fuego == null || !fuego.gameObject.activeInHierarchy) continue;

            Vector3 direccion = fuego.transform.position - camaraJugador.transform.position;
            float distancia = direccion.magnitude;
            if (distancia > rangoApagado) continue;

            float angulo = Vector3.Angle(camaraJugador.transform.forward, direccion);
            if (angulo < 20f)
                return true;
        }

        return false;
    }

    private bool EstaExtintorEquipado()
    {
        if (EquipmentManager.Instance == null)
            return false;

        var equipableObject = EquipmentManager.Instance.objetoActual;
        if (equipableObject == null || equipableObject.itemData == null)
            return false;

        return equipableObject.itemData.id == "extintor";
    }

    private void IntentarApagarFuegos(bool soloApuntado)
    {
        if (fuegos == null || fuegos.Length == 0)
            return;

        if (jugador == null)
            return;

        foreach (ParticleSystem fuego in fuegos)
        {
            if (fuego == null || !fuego.gameObject.activeInHierarchy)
                continue;

            if (soloApuntado && !EstaApuntandoAFuego(fuego))
                continue;

            float distancia = Vector3.Distance(jugador.position, fuego.transform.position);

            if (distancia <= rangoApagado)
            {
                fuego.Stop();
                fuego.gameObject.SetActive(false);
                incendiosApagados++;
            }
        }
    }

    bool EstaApuntandoAFuego(ParticleSystem fuego)
    {
        if (camaraJugador == null) return false;

        Vector3 direccion = fuego.transform.position - camaraJugador.transform.position;
        float angulo = Vector3.Angle(camaraJugador.transform.forward, direccion);
        return angulo < 10f;
    }

    private void OnDestroy()
    {
        if (crosshair != null)
            Destroy(crosshair);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if (fuegoEdificio != null)
            Gizmos.DrawWireSphere(fuegoEdificio.position, rangoApagado);
    }
}