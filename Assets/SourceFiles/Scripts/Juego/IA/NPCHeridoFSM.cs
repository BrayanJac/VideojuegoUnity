using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCHerido))]
[RequireComponent(typeof(DetectorJugador))]
public class NPCHeridoFSM : MonoBehaviour
{
    private NPCHerido npc;
    private DetectorJugador detector;

    private EstadoBase estadoActual;

    private Dictionary<EstadoHerido, EstadoBase> estados;

    private void Awake()
    {
        npc = GetComponent<NPCHerido>();
        detector = GetComponent<DetectorJugador>();

        estados = new Dictionary<EstadoHerido, EstadoBase>()
        {
            { EstadoHerido.Esperando,
                new EstadoEsperando(this, npc, detector) },

            { EstadoHerido.PideAyuda,
                new EstadoPideAyuda(this, npc, detector) },

            { EstadoHerido.RecibePrimerosAuxilios,
                new EstadoRecibePrimerosAuxilios(this, npc) },

            { EstadoHerido.SigueBombero,
                new EstadoSigueBombero(this, npc) },

            { EstadoHerido.Empeora,
                new EstadoEmpeora(this, npc) },

            { EstadoHerido.Rescatado,
                new EstadoRescatado(this, npc) },

            { EstadoHerido.NoRescatado,
                new EstadoNoRescatado(this, npc) }
        };
    }

    private void Start()
    {
        CambiarEstado(EstadoHerido.Esperando);
    }

    private void Update()
    {
        estadoActual?.Actualizar();
    }

    public void CambiarEstado(EstadoHerido nuevoEstado)
    {
        estadoActual?.Salir();

        estadoActual = estados[nuevoEstado];

        estadoActual.Entrar();
    }
}