using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform jugador;

    [Header("Fuegos de la escena")]
    [SerializeField] private List<Transform> fuegos = new List<Transform>();

    [Header("Daño al jugador")]
    private float intervaloDanio = 1f;
    private float cantidadDanio = 10f;
    private float rangoDanio = 25f;

    void Start()
    {
        if (jugador == null)
        {
            GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
            if (jugadorObj != null)
                jugador = jugadorObj.transform;
        }

        StartCoroutine(ComprobarDanioJugador());
    }

    IEnumerator ComprobarDanioJugador()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloDanio);

            if (jugador == null)
            {
                GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
                if (jugadorObj != null)
                    jugador = jugadorObj.transform;

                continue;
            }

            RevisarDistanciaJugador();
        }
    }

    float CalcularRangoEfectivo(Transform fuego)
    {
        return rangoDanio;
    }

    PlayerHealth ObtenerSaludJugador()
    {
        if (PlayerHealth.Instance != null)
            return PlayerHealth.Instance;

        return jugador != null ? jugador.GetComponent<PlayerHealth>() : null;
    }

    void RevisarDistanciaJugador()
    {
        PlayerHealth vidaJugador = ObtenerSaludJugador();

        if (vidaJugador == null)
            return;

        foreach (Transform fuego in fuegos)
        {
            if (fuego == null || !fuego.gameObject.activeInHierarchy)
                continue;

            float distancia = Vector3.Distance(
                jugador.position,
                fuego.position
            );

            float rangoEfectivo = CalcularRangoEfectivo(fuego);

            if (distancia <= rangoEfectivo)
            {
                vidaJugador.RecibirDanio(cantidadDanio);
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        foreach (Transform fuego in fuegos)
        {
            if (fuego != null)
                Gizmos.DrawWireSphere(fuego.position, CalcularRangoEfectivo(fuego));
        }
    }
}
