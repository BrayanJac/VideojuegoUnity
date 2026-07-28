using UnityEngine;
using UnityEngine.InputSystem;

public class ExtintorController : MonoBehaviour
{
    public static int incendiosApagados;
    public static int incendiosTotales;

    private float rangoApagado = 30f;
    [SerializeField] private Transform fuegoEdificio;
    public AudioClip sonidoApagar;

    private Transform[] fuegos;
    private Transform jugador;
    private Camera camaraJugador;

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
            fuegos = new Transform[fuegoEdificio.childCount];
            for (int i = 0; i < fuegoEdificio.childCount; i++)
                fuegos[i] = fuegoEdificio.GetChild(i);
            incendiosTotales = fuegos.Length;
        }

        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }
        camaraJugador = Camera.main;
    }

    private void Update()
    {
        if (!EstaExtintorEquipado())
            return;

        bool apuntandoFuego = EstaApuntandoFuego();

        if (Mouse.current.leftButton.wasPressedThisFrame)
            IntentarApagarFuegos(apuntandoFuego);
    }

    bool EstaApuntandoFuego()
    {
        if (camaraJugador == null || fuegos == null) return false;

        foreach (Transform fuego in fuegos)
        {
            if (fuego == null || !fuego.gameObject.activeInHierarchy) continue;

            Vector3 direccion = fuego.position - camaraJugador.transform.position;
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

        foreach (Transform fuego in fuegos)
        {
            if (fuego == null || !fuego.gameObject.activeInHierarchy)
                continue;

            if (soloApuntado && !EstaApuntandoAFuego(fuego))
                continue;

            float distancia = Vector3.Distance(jugador.position, fuego.position);

            if (distancia <= rangoApagado)
            {
                var particles = fuego.GetComponentsInChildren<ParticleSystem>(true);
                foreach (var ps in particles)
                {
                    ps.Stop();
                    ps.gameObject.SetActive(false);
                }
                incendiosApagados++;

                if (sonidoApagar != null && AudioManager.instancia != null)
                    AudioManager.instancia.ReproducirEfecto(sonidoApagar);
            }
        }
    }

    bool EstaApuntandoAFuego(Transform fuego)
    {
        if (camaraJugador == null) return false;

        Vector3 direccion = fuego.position - camaraJugador.transform.position;
        float angulo = Vector3.Angle(camaraJugador.transform.forward, direccion);
        return angulo < 10f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if (fuegoEdificio != null)
            Gizmos.DrawWireSphere(fuegoEdificio.position, rangoApagado);
    }
}