using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NPCHerido))]
public class NPCHeridoInteractuable : MonoBehaviour
{
    [SerializeField] private float distanciaInteraccion = 8f;

    private bool jugadorCerca;
    private NPCHerido npc;
    private Transform jugador;

    private void Awake()
    {
        npc = GetComponent<NPCHerido>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            jugador = playerObj.transform;
    }

    private void Update()
    {
        if (jugador == null || npc.EstaMuerto)
        {
            if (jugadorCerca)
            {
                jugadorCerca = false;
                if (UIManager.Instance != null)
                    UIManager.Instance.OcultarTextoRecoger();
            }
            return;
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);
        bool estaCerca = distancia <= distanciaInteraccion;

        if (estaCerca && !jugadorCerca)
        {
            jugadorCerca = true;

            if (UIManager.Instance != null)
                UIManager.Instance.MostrarTextoAccion("atender herido");
        }
        else if (!estaCerca && jugadorCerca)
        {
            jugadorCerca = false;

            if (UIManager.Instance != null)
                UIManager.Instance.OcultarTextoRecoger();
        }

        if (!jugadorCerca)
            return;

        if (Keyboard.current == null)
            return;

        if (!Keyboard.current.eKey.wasPressedThisFrame)
            return;

        if (!ObjetoMedico.CurarNPC(npc))
            npc.IniciarPrimerosAuxilios();

        if (UIManager.Instance != null)
            UIManager.Instance.OcultarTextoRecoger();

        jugadorCerca = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}
