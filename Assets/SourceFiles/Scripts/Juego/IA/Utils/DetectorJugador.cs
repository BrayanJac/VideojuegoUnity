using UnityEngine;

[RequireComponent(typeof(NPCHerido))]
public class DetectorJugador : MonoBehaviour
{
    private NPCHerido npc;
    private Transform jugador;

    public bool JugadorDetectado { get; private set; }

    private void Awake()
    {
        npc = GetComponent<NPCHerido>();
    }

    private void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");

        if (obj != null)
            jugador = obj.transform;
    }

    private void Update()
    {
        if (jugador == null)
            return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        JugadorDetectado = distancia <= npc.RadioDeteccion;
    }

    private void OnDrawGizmosSelected()
    {
        NPCHerido datos = GetComponent<NPCHerido>();

        if (datos == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, datos.RadioDeteccion);
    }
}