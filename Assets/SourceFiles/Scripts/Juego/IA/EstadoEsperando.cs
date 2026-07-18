using UnityEngine;

public class EstadoEsperando : EstadoBase
{
    private DetectorJugador detector;

    public EstadoEsperando(NPCHeridoFSM fsm,
                           NPCHerido npc,
                           DetectorJugador detector) : base(fsm, npc)
    {
        this.detector = detector;
    }

    public override void Entrar()
    {
        Debug.Log($"{npc.name}: Esperando");
    }

    public override void Actualizar()
    {
        if (detector.JugadorDetectado)
        {
            fsm.CambiarEstado(EstadoHerido.PideAyuda);
        }
    }
}