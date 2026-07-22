using UnityEngine;

public class EstadoPideAyuda : EstadoBase
{
    private DetectorJugador detector;

    public EstadoPideAyuda(NPCHeridoFSM fsm,
                           NPCHerido npc,
                           DetectorJugador detector) : base(fsm, npc)
    {
        this.detector = detector;
    }

    public override void Entrar()
    {
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirEfecto(npc.SonidoAyuda);

        Debug.Log($"{npc.name}: Ayuda!");
    }

    public override void Actualizar()
    {
        if (npc.EstaMuerto)
            return;

        if (detector != null && !detector.JugadorDetectado)
        {
            fsm.CambiarEstado(EstadoHerido.Esperando);
            return;
        }

        bool critico = npc.PacienteCritico;
        bool vidaBaja = npc.Salud != null && npc.Salud.PorcentajeVida <= 0.5f;

        if (critico || vidaBaja)
            fsm.CambiarEstado(EstadoHerido.Empeora);
    }
}
