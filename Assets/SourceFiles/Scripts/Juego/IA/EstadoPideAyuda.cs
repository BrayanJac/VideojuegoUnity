using UnityEngine;

public class EstadoPideAyuda : EstadoBase
{
    public EstadoPideAyuda(NPCHeridoFSM fsm,
                           NPCHerido npc) : base(fsm, npc)
    {
    }

    public override void Entrar()
    {
        if (AudioManager.instancia != null)
            AudioManager.instancia.ReproducirEfecto(npc.SonidoAyuda);

        Debug.Log($"{npc.name}: ¡Ayuda!");
    }

    public override void Actualizar()
    {
        if (npc.EstaMuerto)
            return;

        // Paciente critico o vida <= 50% ? empeora.
        bool critico = npc.PacienteCritico;
        bool vidaBaja = npc.Salud != null && npc.Salud.PorcentajeVida <= 0.5f;

        if (critico || vidaBaja)
            fsm.CambiarEstado(EstadoHerido.Empeora);
    }
}
